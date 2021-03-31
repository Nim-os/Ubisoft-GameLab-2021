using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class TutorialManager : MonoBehaviour
{
    GameObject player;
    public InputSystem input;

    int state = 0;
    private int lastState = -1;
    List<Image> markers = new List<Image>();

    [SerializeField]
    private GameObject basicPlanet;
    [SerializeField]
    private GameObject basicAsteroid;

    public GameObject sun;
    GameObject firstPlanet;
    public GameObject gravPlanetCluster;
    public GameObject massAbsorpCluster;
    public GameObject endCluster;
    public Canvas canv;
    public Image arrow;

    private Camera mainCamera;
    private PhotonView photonView;

    bool holdingRMB = false;
    bool sunChasing = false;
    bool escaped = false;
    float previousMass;
    float threshold;

    void Awake()
    {
        input = new InputSystem();

        input.Game.Secondary.performed += x => holdingRMB = true;
        input.Game.Secondary.canceled += x => holdingRMB = false;

        mainCamera = Camera.main;
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (player == null) // This may be better suited inside Awake
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                if (p.GetComponent<PhotonView>().IsMine) player = p;
            }
        }
        else
        {

            // Sun chases players.
            if (sunChasing)
            {
                if (Vector3.Distance(player.transform.position, sun.transform.position) > 50) sun.transform.position = new Vector3(sun.transform.position.x + (9 * Time.deltaTime), 0, player.transform.position.z);
                else sun.transform.position = new Vector3(sun.transform.position.x + (1 * Time.deltaTime), 0, player.transform.position.z);
            }
            

            // Update state if we change states
            if (state != lastState)
			{
                // Edge case for first state
                if (state != 0)
                {
                    // Update other's states
                    photonView.RPC("UpdateState", RpcTarget.Others, new object[] { state });
                }

                // Update our own state
                UpdateState(state);
            }


            if (state == 0) GravitationToPlayer(); // Approach other player with RMB/propulse
            else if (state == 1) Propulsion(); // Propulse with LMB
            else if (state == 2) GravitationToPlanet(); // Gravitate to basic planet with RMB
            else if (state == 3) MassAbsorption(); // Absorb small mass on collision
            else if (state == 4) MassEjection(); // Eject mass with 'z'
            else if (state == 5) Escape(); // Escape an obstacle field with sun enabled
        }
    }

    void SetUp(string prompt) // Set up tutorial stage by activating UI prompt and setting prompt text.
    {
        canv.transform.GetChild(state).gameObject.SetActive(true);
        Image currentMarker = canv.transform.GetChild(state).gameObject.GetComponent<Image>();
        currentMarker.GetComponentInChildren<Text>().text = prompt;
        markers.Add(currentMarker);
    }
    void TearDown() // Deactivate completed task's UI prompt. Proceed to next tutorial stage.
    {
        foreach (Transform t in markers[state].transform)
        {
            t.gameObject.SetActive(false);
        }
        markers[state].gameObject.SetActive(false);
        state++;
    }

    /// <summary>
    /// Updates the local state
    /// </summary>
    /// <param name="newState">The new state</param>
    [PunRPC]
    private void UpdateState(int newState)
    {
        // Checks if we have already updated to the new state so to not clobber any outgoing message
        if (lastState != newState)
        {
            // Check if we are the receiver
            if (state != newState)
            {
                TearDown();

                // Just in case we drop a packet
                state = newState;
            }

            // Sync lastState
            lastState = newState;
        }
    }

    #region Tutorial Stages
    void GravitationToPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (markers.Count == state)
        {
            SetUp("Hold RMB to gravitate to your friend. You can take turns propulsing and gravitating to save resources. Careful: Don't crash!");
        }
        // Proceeds to the next stage if players are close enough wehether due to gravity or propulsion.
        else if (players.Length < 2) return;
        else if (holdingRMB && player.GetComponent<PlayerGravitation>().currentSelection.gameObject.tag == "Player" && Vector3.Distance(players[0].transform.position, players[1].transform.position) < 15 && markers[state].enabled)
        {
            TearDown();
            previousMass = player.GetComponent<Rigidbody>().mass;
        }
    }
    void Propulsion()
    {
        if (markers.Count == state)
        {
            SetUp("Hold/Tap LMB to propulse.");
            threshold = player.transform.position.x;
        }
        else if (player.transform.position.x > (threshold + 30) && markers[state].enabled)
        {
            TearDown();
        }
    }
    void GravitationToPlanet()
    {
        if (markers.Count == state)
        {
            SetUp("Hold RMB to gravitate.");
            firstPlanet = Instantiate(basicPlanet, new Vector3(player.transform.position.x + 45, 0, player.transform.position.z - 12), Quaternion.identity);
            firstPlanet.transform.localScale = new Vector3(5, 5, 5);
            firstPlanet.GetComponent<Rigidbody>().mass = 8;
            gravPlanetCluster.transform.position = firstPlanet.transform.position + new Vector3(60, 0, -15);
            gravPlanetCluster.SetActive(true);
        }
        if (player.transform.position.x > (gravPlanetCluster.transform.position.x + 80))
        {
            TearDown();
            gravPlanetCluster.SetActive(false);
        }
        else if (!(holdingRMB && player.GetComponent<PlayerGravitation>().currentSelection != null && markers[state].enabled))
        {
            canv.transform.GetChild(state).transform.position = Camera.main.WorldToScreenPoint(firstPlanet.transform.position);
        }
        if (player.GetComponent<PlayerGravitation>().currentSelection.gameObject.tag != "Player")
        {
            canv.transform.GetChild(state).transform.GetChild(0).GetComponent<Text>().text = "Keep going!";
        }
    }
    void MassEjection()
    {
        if (markers.Count == state)
        {
            SetUp("Hold and release z to eject mass.");
        }
        else if (player.GetComponent<PlayerProjectile>().holding && markers[state].enabled)
        {
            TearDown();
        }
        else previousMass = player.GetComponent<Rigidbody>().mass;
    }
    void MassAbsorption()
    {
        if (markers.Count == state)
        {
            SetUp("Collide with mass smaller than yourself to absorb it.");
            // Spawn some asteroids.
            // TODO: Allow instantiations from one player to be seen as ghostly/uninteractable obstacles on the other player's screen.
            float tempX = player.transform.position.x;
            int j = 5;
            for (int i = 0; i < j; i++)
            {
                Instantiate(basicAsteroid, new Vector3((tempX + 50) + (i * 10), 1, player.transform.position.z), Quaternion.identity);
            }
            threshold = tempX + 25 + (j * 10);

            //massAbsorpCluster.transform.position = new Vector3(tempX + 60, 0, 15);
            //massAbsorpCluster.SetActive(true);
        }
        // Proceeds to next stage after player is mostly through the asteroid field. 
        else if (player.transform.position.x > threshold && markers[state].enabled)
        {
            TearDown();
        }
    }
    void Escape()
    {
        if (markers.Count == state)
        {
            SetUp("Escape the solar system!");
            sunChasing = true;
            sun.transform.position = new Vector3(player.transform.position.x - 100, 1, player.transform.position.z);
            endCluster.SetActive(true);
            endCluster.transform.position = new Vector3(player.transform.position.x + 60, 1, player.transform.position.z);
        }
        else if (player.transform.position.x >= endCluster.transform.position.x + 100)
        {
            sunChasing = false;
            
            // Check if we have already tried to change scenes
            if (!escaped)
			{
                escaped = true;

                ServerManager.instance.LoadRoomLevel(3);
			}
        }
    }

    #endregion

    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    public void RestartGame()
    {
        ServerManager.instance.Close();

        SceneManager.LoadScene(0);
    }
}

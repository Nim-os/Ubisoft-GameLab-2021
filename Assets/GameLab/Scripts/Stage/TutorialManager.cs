using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    GameObject player;
    public InputSystem input;

    int state = 0;
    List<Image> markers = new List<Image>();

    [SerializeField]
    private GameObject basicPlanet;
    [SerializeField]
    private GameObject basicAsteroid;

    public GameObject sun;
    GameObject firstPlanet;
    public GameObject endCluster;
    public Canvas canv;
    public Image arrow;

    bool holdingRMB = false;
    bool sunChasing = false;
    float previousMass;

    void Awake()
    {
        input = new InputSystem();

        input.Game.Secondary.performed += x => holdingRMB = true;
        input.Game.Secondary.canceled += x => holdingRMB = false;
    }

    void Update()
    {
        if (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                if (p.GetComponent<Photon.Pun.PhotonView>().IsMine) player = p;
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

            if (state == 0) GravitationToPlayer(); // Approach other player with RMB/propulse
            if (state == 1) Propulsion(); // Propulse with LMB
            if (state == 2) GravitationToPlanet(); // Gravitate to basic planet with RMB
            if (state == 3) MassEjection(); // Eject mass with 'z'
            if (state == 4) MassAbsorption(); // Absorb small mass on collision
            if (state == 5) Escape(); // Escape an obstacle field with sun enabled

            // Players do not have to worry about gas in stage 0 and 1.
            if (!canv.transform.GetChild(6).gameObject.activeInHierarchy) player.GetComponent<PlayerPropulsion>().gas = 100;
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

    #region Tutorial Stages
    void GravitationToPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        canv.transform.GetChild(6).gameObject.SetActive(false); 

        if (markers.Count == state)
        {
            SetUp("Hold RMB to gravitate to your friend. You can take turns propulsing and gravitating to save resources. Careful: Don't crash!");
        }
        // Proceeds to the next stage if players are close enough wehether due to gravity or propulsion.
        else if (players.Length < 2) return;
        else if (holdingRMB && player.GetComponent<PlayerGravitation>().currentSelection != null && Vector3.Distance(players[0].transform.position, players[1].transform.position) < 15 && markers[state].enabled)
        {
            TearDown();
            previousMass = player.GetComponent<Rigidbody>().mass;
        }
    }
    void Propulsion()
    {
        if (markers.Count == state)
        {
            SetUp("Tap/Hold LMB to propulse.");
        }
        else if (player.transform.position.x > 75 && markers[state].enabled)
        {
            TearDown();
            canv.transform.GetChild(6).gameObject.SetActive(true); // Players start to worry about gas.
        }
    }
    void MassAbsorption()
    {
        if (markers.Count == state)
        {
            SetUp("Colliding with mass smaller than you will replenish your fuel.");
            // Spawn some asteroids.
            // TODO: Allow instantiations from one player to be seen as ghostly/uninteractable obstacles on the other player's screen.
            for (int i = 0; i < 5; i++)
            {
                Instantiate(basicAsteroid, new Vector3(100 + (i * 10), 1, player.transform.position.z), Quaternion.identity);
            }
        }
        // Proceeds to next stage after player is mostly through the asteroid field. 
        else if (player.transform.position.x > 150 && markers[state].enabled)
        {
            TearDown();
            // Instantiate a planet to be gravitated towards in the next stage, providing a visible goal to approach in the current stage.
            firstPlanet = Instantiate(basicPlanet, new Vector3(175, 1, 0), Quaternion.identity);
            firstPlanet.transform.localScale = new Vector3(6, 6, 6);
            firstPlanet.GetComponent<Rigidbody>().mass = 8;
        }
    }
    void GravitationToPlanet()
    {
        if (markers.Count == state)
        {
            SetUp("Hold RMB to gravitate.");
        }
        else if (holdingRMB && player.GetComponent<PlayerGravitation>().currentSelection != null && markers[state].enabled)
        {
            TearDown();
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
    void Escape()
    {
        if (markers.Count == state)
        {
            SetUp("Escape the solar system!");
            sunChasing = true;
            sun.transform.position = new Vector3(75, 1, player.transform.position.z);
            endCluster.SetActive(true);
            endCluster.transform.position = new Vector3(player.transform.position.x + 25, 1, player.transform.position.z);
        }
        else if (player.transform.position.x >= endCluster.transform.position.x + 100)
        {
            sunChasing = false;
            // Go to next scene.
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
        SceneManager.LoadScene(0);
    }
}

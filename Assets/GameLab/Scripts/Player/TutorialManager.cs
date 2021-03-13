using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    GameObject player;
    public InputSystem input;

    int state = 0;
    List<Image> markers = new List<Image>();
    
    public GameObject basicPlanet;
    public GameObject basicAsteroid;
    public GameObject sun;
    GameObject firstPlanet;
    public Canvas canv;
    public Image arrow;
    GameObject testPlayer; // For testing.

    bool holdingRMB = false;
    bool sunChasing = true;
    float previousMass;

    void Awake()
    {
        input = new InputSystem();

        input.Game.Secondary.performed += x => holdingRMB = true;
        input.Game.Secondary.canceled += x => holdingRMB = false;
    }

    private void Start()
    {
        firstPlanet = Instantiate(basicPlanet, new Vector3(50, 1, 0), Quaternion.identity);
        firstPlanet.transform.localScale = new Vector3(3, 3, 3);
        firstPlanet.GetComponent<Rigidbody>().mass = 999;
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

            if (state == 0) // Propulsion
            {
                // TODO: Absorption script still runs --> Need to add isEnabled check to PlayerAbsorption.cs

                // Set up stage
                if (markers.Count == state)
                {
                    SetUp("Hold/Tap LMB to propulse.");
                }
                // Condition to proceed
                else if ((Camera.main.WorldToScreenPoint(firstPlanet.transform.position).x < Screen.width - 40) && markers[state].enabled)
                {
                    TearDown();
                }
            }

            if (state == 1) // Gravitation
            {
                // Set up stage
                if (markers.Count == state)
                {
                    SetUp("Hold RMB to gravitate.");
                }
                // Condition to proceed
                else if (holdingRMB && player.GetComponent<PlayerGravitation>().currentSelection != null && markers[state].enabled)
                {
                    TearDown();
                }
            }

            if (state == 2) // Gravitation between players
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                // Set up stage
                if (markers.Count == state)
                {
                    SetUp("Hold RMB to gravitate to your friend. You can take turns propulsing and gravitating to save resources. Careful: Don't crash!");
                    if (players.Length < 2)
                    {
                        testPlayer = Instantiate(basicPlanet, new Vector3(100, 1, player.transform.position.z - 10), Quaternion.identity);
                        testPlayer.tag = "Player";
                        players = new GameObject[] { player, testPlayer };
                    }
                }
                // Condition to proceed
                else if (Vector3.Distance(players[0].transform.position, players[1].transform.position) < 15 && markers[state].enabled)
                {
                    TearDown();
                    Destroy(testPlayer);
                    previousMass = player.GetComponent<Rigidbody>().mass;
                }
            }

            if (state == 3) // Mass Ejection
            {
                // Set up stage
                if (markers.Count == state)
                {
                    SetUp("Hold and release z to eject mass.");
                }
                // Condition to proceed // TODO: Check that mass was lost due to ejection rather than propulsion. Input system?
                else if (player.GetComponent<Rigidbody>().mass < previousMass && markers[state].enabled)
                {
                    TearDown();
                    previousMass = player.GetComponent<Rigidbody>().mass;
                }
                else previousMass = player.GetComponent<Rigidbody>().mass;

            }

            if (state == 4) // Mass Absorption
            {
                // Set up stage
                if (markers.Count == state)
                {
                    SetUp("Collide with mass smaller than yourself to absorb it.");
                    // Spawn some asteroids.
                    Instantiate(basicAsteroid, new Vector3(130, 1, player.transform.position.z + 5), Quaternion.identity);
                    Instantiate(basicAsteroid, new Vector3(140, 1, player.transform.position.z - 5), Quaternion.identity);
                    Instantiate(basicAsteroid, new Vector3(150, 1, player.transform.position.z + 5), Quaternion.identity);
                    Instantiate(basicAsteroid, new Vector3(160, 1, player.transform.position.z - 5), Quaternion.identity);
                    Instantiate(basicAsteroid, new Vector3(170, 1, player.transform.position.z + 5), Quaternion.identity);
                }
                // Condition to proceed
                else if (player.GetComponent<Rigidbody>().mass > previousMass && markers[state].enabled)
                {
                    TearDown();
                }
                else previousMass = player.GetComponent<Rigidbody>().mass;
            }

            if (state == 5) // Navigate to the end
            {
                // Set up stage
                if (markers.Count == state)
                {
                    SetUp("Escape the solar system!");
                }
                // Condition to proceed
                else if (player.transform.position.x >= 9999)
                {
                    sunChasing = false;
                    // Go to next scene.
                }
            }
        }
    }

    void SetUp(string prompt)
    {
        canv.transform.GetChild(state).gameObject.SetActive(true);
        Image currentMarker = canv.transform.GetChild(state).gameObject.GetComponent<Image>();
        currentMarker.GetComponentInChildren<Text>().text = prompt;
        markers.Add(currentMarker);
    }
    void TearDown()
    {
        foreach (Transform t in markers[state].transform)
        {
            t.gameObject.SetActive(false);
        }
        markers[state].gameObject.SetActive(false);
        state++;
    }

    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
}

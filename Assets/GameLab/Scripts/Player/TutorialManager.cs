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
    GameObject firstPlanet;
    public Canvas canv;
    public Image arrow;

    private bool holdingRMB, propulsing = false;

    void Awake()
    {
        input = new InputSystem();

        input.Game.Secondary.performed += x => holdingRMB = true;
        input.Game.Secondary.canceled += x => holdingRMB = false;

        input.Game.Primary.performed += x => propulsing = true;
        input.Game.Primary.canceled += x => propulsing = false;
    }

    private void Start()
    {
        firstPlanet = Instantiate(basicPlanet, new Vector3(50, 1, 0), Quaternion.identity);
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

        if (state == 0) // Propulsion
        {
            player.GetComponent<PlayerAbsorption>().enabled = false;
            // TODO: Absorption script still runs --> Need to add isEnabled check to PlayerAbsorption.cs

            // Set up stage
            if (markers.Count == state)
            {
                SetUp("Hold/Tap LMB to propulse.");
                // TODO: Spawn sun to the left of the scene.
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
                // TODO: Spawn sun to the left of the scene.
            }
            //Condition to proceed
            else if (holdingRMB && player.GetComponent<PlayerGravitation>().currentSelection != null && markers[state].enabled)
            {
                TearDown();
            }
        }

        if (state == 2) // Gravitation between players
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (markers.Count == state)
            {
                SetUp("Hold RMB to gravitate to your friend. You can take turns propulsing and gravitating to save resources. Careful: Don't crash!");
                // TODO: Spawn sun to the left of the scene.
            }
            //Condition to proceed
            else if ((holdingRMB || propulsing) && Vector3.Distance(players[0].transform.position, players[1].transform.position) < 5 && markers[state].enabled)
            {
                TearDown();
            }
        }

        if (state == 3) // Mass Ejection
        {
            // Set up stage
            if (markers.Count == state) // TODO: Make text element in Unity inspector.
            {
                SetUp("Hold and release _ to eject mass.");
                // TODO: Spawn sun to the left of the scene.
            }
            //Condition to proceed
            //if (mass ejected && markers[state].enabled)
            {
                TearDown();
            }

        }

        if (state == 4) // Mass Absorption
        {
            // Set up stage
            if (markers.Count == state) // TODO: Make arrow element towards asteroid field in Unity inspector.
            {
                SetUp("Collide with mass smaller than yourself to absorb it.");
                // TODO: Spawn sun to the left of the scene.
            }
            //Condition to proceed
            //if (player mass increased)
            {
                TearDown();
            }
        }

        if (state == 5) // Navigate to the end
        {
            // Set up stage
            if (markers.Count == state) // TODO: Make arrow element towards obstacle field in Unity inspector.
            {
                SetUp("Escape the solar system!");
                // TODO: Spawn sun to the left of the scene.
            }
            //Condition to proceed
            if (player.transform.position.x >= 9999)
            {
                // Go to next scene.
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
        markers[state].enabled = false;
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

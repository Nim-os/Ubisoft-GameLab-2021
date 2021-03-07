using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    //GameObject player;

    int state = 0;
    List<Image> markers = new List<Image>();
    
    public GameObject basicPlanet;
    GameObject firstPlanet;
    public Canvas canv;
    public Image arrow;

    private void Start()
    {
        /*GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<Photon.Pun.PhotonView>().IsMine) player = p;
        }*/

        firstPlanet = Instantiate(basicPlanet, new Vector3(50, 1, 0), Quaternion.identity);
    }

    void Update()
    {
        if (state == 0)
        {
            //player.GetComponent<PlayerAbsorption>().enabled = false;

            if ((Camera.main.WorldToScreenPoint(firstPlanet.transform.position).x < Screen.width - 40) && markers[0].enabled)
            {
                foreach (Transform t in markers[0].transform)
                {
                    t.gameObject.SetActive(false);
                }
                markers[0].enabled = false;
                state++;
            }
            else if (markers.Count == 0)
            {
                canv.transform.GetChild(0).gameObject.SetActive(true);
                Image currentArrow = canv.transform.GetChild(0).gameObject.GetComponent<Image>();
                currentArrow.GetComponentInChildren<Text>().text = "Left-Click to propulse.";
                markers.Add(currentArrow);
            }
        }
    }
}

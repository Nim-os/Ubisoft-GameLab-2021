using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ping : MonoBehaviour
{
    public GameObject playerPing;
    public GameObject hazardPing;
    public GameObject locationPing;
    public Canvas canv;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Vector2 mousePos = Input.mousePosition;
            GameObject ping = locationPing;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag.Equals("Player")) ping = playerPing;
                if (hit.collider.gameObject.tag.Equals("Hazard")) ping = hazardPing;
            }

            GameObject marker;

            if (PhotonNetwork.InRoom)
			{
                marker = PhotonNetwork.Instantiate(ping.name, mousePos, Quaternion.identity);
            }
            else
            {
                marker = Instantiate(ping, mousePos, Quaternion.identity);
            }

            marker.transform.SetParent(canv.transform);
        }
    }
}

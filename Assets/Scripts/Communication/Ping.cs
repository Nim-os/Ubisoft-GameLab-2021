using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ping : MonoBehaviour
{
    public static Ping instance { get; private set; }

    public LayerMask layer;
    public GameObject playerPing;
    public GameObject hazardPing;
    public GameObject locationPing;
    public Canvas canv;

	private void Awake()
	{
		if (instance == null)
		{
            instance = this;
		}
        else
		{
            Destroy(this);
            return;
		}
	}

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SendPing();
        }
    }

    void SendPing()
	{
        Vector2 mousePos = Input.mousePosition;
        GameObject ping = locationPing;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layer, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.tag.Equals("Player")) ping = playerPing;
            if (hit.collider.gameObject.tag.Equals("Hazard")) ping = hazardPing;
        }


        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.Instantiate(ping.name, ray.origin, Quaternion.identity);
        }
    }

    public Canvas GetCanvas()
	{
        return canv;
	}
}

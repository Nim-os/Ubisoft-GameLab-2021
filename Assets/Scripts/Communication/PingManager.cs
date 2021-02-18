using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PingManager : MonoBehaviour
{
    public static PingManager instance { get; private set; }

    public InputSystem input;

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

        input = new InputSystem();

        input.Game.Ping.performed += x => SendPing();
	}

    void SendPing()
	{
        Debug.Log("Pressed0");
        Debug.Log("Pressed1");
        Debug.Log("Pressed2");
        Debug.Log("Pressed3");
        GameObject ping = locationPing;

        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue());

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

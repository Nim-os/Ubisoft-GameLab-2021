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

    private Vector2 mousePos;

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
        input.Game.MousePosition.performed += x => mousePos = x.ReadValue<Vector2>();
	}

    void SendPing()
    {
        if (PhotonNetwork.InRoom)
        {
            GameObject ping = locationPing;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, layer, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject.CompareTag("Player")) ping = playerPing;
                if (hit.collider.gameObject.CompareTag("Hazard")) ping = hazardPing;
            }

            // Don't use ray.origin for placing the pings
            // That position is where the camera is placed and not the point the player wants to ping
            PhotonNetwork.Instantiate(ping.name, ray.GetPoint(Camera.main.transform.position.y), Quaternion.identity);
        }
    }

    public Canvas GetCanvas()
	{
        return canv;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupMarker : MonoBehaviour
{
    Image pickupsMarker;
    public GameObject pickupsParent;
    List<GameObject> pickups = new List<GameObject>();
    public GameObject closestPickup;

    // Distance for marker to edge of screen.
    int buffer = 50;

    private void Start()
    {
        for (int i = 0; i < pickupsParent.transform.childCount; i++)
        {
            pickups.Add(pickupsParent.transform.GetChild(i).gameObject);
        }
        pickupsMarker = GetComponent<Image>();
        closestPickup = pickups[0];
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length != 0)
        {
            // Set average player position to find closest key.
            Vector3 midpoint;
            if (players.Length == 1) midpoint = players[0].transform.position;
            else midpoint = (players[0].transform.position + players[1].transform.position) / 2;

            float minDistance = 1000;

            // Find closest key to players.
            foreach (GameObject g in pickups)
            {
                if (g == null)
                {
                    pickups.Remove(g);
                    continue;
                }
                if (Vector3.Distance(g.transform.position, midpoint) < minDistance)
                {
                    closestPickup = g;
                    minDistance = Vector3.Distance(g.transform.position, midpoint);
                }
            }

            // Set marker position.
            pickupsMarker.rectTransform.position = Camera.main.WorldToScreenPoint(closestPickup.transform.position);
            if (pickupsMarker.rectTransform.position.x > Screen.width - buffer) pickupsMarker.rectTransform.position = new Vector2(Screen.width - buffer, pickupsMarker.rectTransform.position.y);
            if (pickupsMarker.rectTransform.position.x < buffer) pickupsMarker.rectTransform.position = new Vector2(buffer, pickupsMarker.rectTransform.position.y);
            if (pickupsMarker.rectTransform.position.y > Screen.height - buffer) pickupsMarker.rectTransform.position = new Vector2(pickupsMarker.rectTransform.position.x, Screen.height - buffer);
            if (pickupsMarker.rectTransform.position.y < buffer) pickupsMarker.rectTransform.position = new Vector2(pickupsMarker.rectTransform.position.x, buffer);
            // Set marker rotation.
            pickupsMarker.rectTransform.LookAt(Camera.main.WorldToScreenPoint(closestPickup.transform.position), Vector3.forward);
            pickupsMarker.rectTransform.eulerAngles = new Vector3(0, 0, pickupsMarker.rectTransform.eulerAngles.z - 180);

            if (closestPickup != null && closestPickup.GetComponent<MeshRenderer>().isVisible) pickupsMarker.enabled = false;
            else pickupsMarker.enabled = true;
        }
        // Remove marker when all keys collected.
        if (pickups.Count == 0 && pickupsMarker != null) Destroy(pickupsMarker);
    }
}

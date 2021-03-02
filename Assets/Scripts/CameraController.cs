using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject[] players;
    private CinemachineTransposer cameraTransposer;
    private float cameraHeight;

    void Start()
    {
        cameraTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        cameraHeight = cameraTransposer.m_FollowOffset.y;
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        float minDistance = Vector3.Distance(Camera.main.ScreenToWorldPoint(Vector3.zero), Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0))); // Get smallest distance required to be visible.
        float buffer = 5; // Applies buffer between planets and edge of screen.

        for (int i = 0; i < players.Length - 1; i++) // Moves the camera up (away) or down (closer) to keep players on-screen.
        // TODO: Test this.
        {
            if (players[i].GetComponent<Renderer>().isVisible)
            {
                for (int j = 0; j < players.Length; j++)
                {
                    if (i == j || !(players[i].GetComponent<Renderer>().isVisible) || !(players[j].GetComponent<Renderer>().isVisible)) continue;
                    if (minDistance > Vector3.Distance(players[i].transform.position, players[j].transform.position) + buffer) SetCameraHeight(cameraHeight - 0.05f);
                }
            }
            else SetCameraHeight(cameraHeight + 0.05f);
        }
    }

    private void SetCameraHeight(float height)
    {
        cameraTransposer.m_FollowOffset.y = height;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject[] players;
    private CinemachineTransposer cameraTransposer;
    private float cameraHeight;
    public float minDistance;

    void Start()
    {
        cameraTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        cameraHeight = 25;
        SetCameraHeight(cameraHeight);
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        // Add player to cameraTargetGroup
        var cameraTargetGroup = GameObject.Find("CameraTargetGroup").GetComponent<CinemachineTargetGroup>();
        foreach (GameObject g in players) cameraTargetGroup.AddMember(g.transform, 1, 0);

        minDistance = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, cameraHeight)), Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, cameraHeight))); ; // Get smallest distance required to be visible.

        // Moves the camera up (away) or down (closer) to keep players on-screen.
        if (players.Length == 2)
        {
            float distance = Vector3.Distance(players[0].transform.position, players[1].transform.position);
            if ((distance < (0.75f * minDistance)) && (cameraHeight >= 25))
            {
                cameraHeight = cameraHeight - 0.5f;
                SetCameraHeight(cameraHeight);
            }
            else if ((distance > (minDistance)) && (cameraHeight <= 500))
            {
                cameraHeight = cameraHeight + 0.5f;
                SetCameraHeight(cameraHeight);
            }
        }
    }

    private void SetCameraHeight(float height)
    {
        cameraTransposer.m_FollowOffset.y = height;
    }
}

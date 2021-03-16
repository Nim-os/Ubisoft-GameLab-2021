using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject[] players;
    private CinemachineTransposer cameraTransposer;
    private CinemachineTargetGroup cameraTargetGroup;
    private float cameraHeight; // Current camera height.
    public float cameraHeightThreshold = 200; // Camera will not zoom out past this point.
    public float minDistance;  // Smallest distance required to be visible.
    public float distanceBuffer = 50; // Extra space around players to better see nearby obstacles.

    void Start()
    {
        cameraTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        cameraHeight = 25;
        SetCameraHeight(cameraHeight);


        // Add players to cameraTargetGroup
        players = GameObject.FindGameObjectsWithTag("Player");
        cameraTargetGroup = GameObject.Find("CameraTargetGroup").GetComponent<CinemachineTargetGroup>();
        StartCoroutine(WaitTwoPlayers());
    }

    void Update()
    {
        if (players.Length <= 2 && cameraTargetGroup.m_Targets.Length < 2) players = GameObject.FindGameObjectsWithTag("Player");

        else
        {
            minDistance = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, cameraHeight)), Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, cameraHeight))) - distanceBuffer;

            // Moves the camera up (away) or down (closer) to keep players on-screen.
            if (players.Length == 2)
            {
                float distance = Vector3.Distance(players[0].transform.position, players[1].transform.position);
                if ((distance <= (minDistance)) && (cameraHeight >= cameraHeightThreshold))
                {
                    cameraHeight = cameraHeight - 0.5f;
                    SetCameraHeight(cameraHeight);
                }
                else if ((distance > (minDistance)) && (cameraHeight < cameraHeightThreshold))
                {
                    cameraHeight = cameraHeight + 0.5f;
                    SetCameraHeight(cameraHeight);
                }
            }
        }
    }

    private void SetCameraHeight(float height)
    {
        cameraTransposer.m_FollowOffset.y = height;
    }

    IEnumerator WaitTwoPlayers() // Makes sure both players are included in the camera target group.
    {
        yield return new WaitUntil(() => players.Length == 2);
        foreach (GameObject g in players)
        {
            cameraTargetGroup.AddMember(g.transform, 1, 0);
        }
    }
}

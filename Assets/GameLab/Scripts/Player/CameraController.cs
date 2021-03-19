using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject[] players;
    private CinemachineTransposer cameraTransposer;
    private CinemachineTargetGroup cameraTargetGroup;
    public ServerManager sm;
    private float cameraHeight; // Current camera height.
    public float cameraHeightThreshold = 200; // Camera will not zoom out past this point.
    public float minDistance;  // Smallest distance required to be visible.

    private GameObject parallax;

    void Start()
    {
        cameraTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        cameraHeight = 25;
        SetCameraHeight(cameraHeight);

        parallax = Camera.main.transform.GetChild(0).gameObject;

        // Add players to cameraTargetGroup
        cameraTargetGroup = GameObject.Find("CameraTargetGroup").GetComponent<CinemachineTargetGroup>();

        StartCoroutine(WaitTwoPlayers());
    }

    void Update()
    {
        if (cameraTargetGroup.m_Targets.Length < 2) return;

        else
        {
            minDistance = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, cameraHeight)), Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, cameraHeight)));

            // Moves the camera up (away) or down (closer) to keep players on-screen.
            if (players.Length == 2)
            {
                float distance = Vector3.Distance(players[0].transform.position, players[1].transform.position);
                if ((distance <= (0.5f * minDistance)) && (cameraHeight >= (0.25f * cameraHeightThreshold)))
                {
                    cameraHeight = cameraHeight - 0.5f;
                    parallax.transform.position = new Vector3(parallax.transform.position.x, parallax.transform.position.y + 0.5f, parallax.transform.position.z);
                    SetCameraHeight(cameraHeight);
                }
                else if ((distance > (minDistance)) && (cameraHeight < cameraHeightThreshold))
                {
                    cameraHeight = cameraHeight + 0.5f;
                    parallax.transform.position = new Vector3(parallax.transform.position.x, parallax.transform.position.y - 0.5f, parallax.transform.position.z);
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
        if (sm.serverMode == ServerManager.Mode.Online)
        {
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == 2);
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject g in players)
            {
                cameraTargetGroup.AddMember(g.transform, 1, 0);
            }
        }

        else
        {
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == 1);
            cameraTargetGroup.AddMember(GameObject.FindGameObjectWithTag("Player").transform, 1, 0);
        }
    }
}

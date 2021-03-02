using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks
{
	public const string DevRoomID = "dev";
    public Mesh p2_1;
    public Material p2_1_mat;

    void Start()
	{
		Debug.Log("Attempting to connect");

		PhotonNetwork.ConnectUsingSettings();
	}

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Photon master server");

		Debug.Log("Attempting to join room");

		PhotonNetwork.JoinRoom(DevRoomID);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Successfully joined room");

		GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-10,10), 1, Random.Range(-10, 10)), Quaternion.identity);
        if (Random.Range(0, 2) == 1)
        {
            Mesh p2_1_inst = Instantiate(p2_1);
            player.GetComponent<MeshFilter>().mesh = p2_1_inst;
            player.GetComponent<MeshRenderer>().material = p2_1_mat;
        }
    }

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.Log($"Failed to join room.\nError message: {returnCode}, {message}");

		Debug.Log("Attempting to create room");

		CreateDevRoom();
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Successfully created room");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log($"Failed to create room.\nError message: {message}");
	}


	private void CreateDevRoom()
	{
		var room = new RoomOptions()
		{
			IsOpen = true,
			IsVisible = false,
			MaxPlayers = 12
		};

		PhotonNetwork.CreateRoom(DevRoomID, room);
	}
}

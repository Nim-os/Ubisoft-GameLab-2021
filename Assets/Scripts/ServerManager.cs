using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ServerManager : MonoBehaviourPunCallbacks
{
	public const string DevRoomID = "dev";

	void Start()
	{
		Debug.Log("Attempting to connect");

		PhotonNetwork.ConnectUsingSettings();
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
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.Log($"Failed to join room.\nError message: {returnCode}, {message}");

		Debug.Log("Attempting to create room");

		PhotonNetwork.CreateRoom(DevRoomID);
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Successfully created room");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log($"Failed to create room.\nError message: {message}");
	}
}

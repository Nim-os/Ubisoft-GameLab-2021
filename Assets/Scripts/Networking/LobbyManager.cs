using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	public const string DevRoomID = "dev";

	public TextMeshProUGUI logPanel;
	public TextMeshProUGUI roomCode;

	//public Mesh p2_1;
	//public Material p2_1_mat;

	#region Buttons

	public void CreateRoom()
	{
		Log($"Creating room...");

		var room = new RoomOptions()
		{
			IsOpen = true,
			IsVisible = false,
			MaxPlayers = 2
		};

		// TODO

		PhotonNetwork.CreateRoom("aa", room);
	}

	public void JoinRoom()
	{
		Log($"Joining room {roomCode.text}...");


		PhotonNetwork.JoinRoom(roomCode.text);
	}

	public void PlayGame()
	{
		Log("Starting game...");

		// TODO Players and min player checking
	}

	public string CreateDevRoom()
	{
		var room = new RoomOptions()
		{
			IsOpen = true,
			IsVisible = false,
			MaxPlayers = 12
		};

		PhotonNetwork.CreateRoom(DevRoomID, room);

		return DevRoomID;
	}

	#endregion


	#region Callbacks

	public override void OnCreatedRoom()
	{
		Debug.Log("Successfully created room");

		Log($"Created room ");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.LogError($"Failed to create room.\nError message: {message}");

		Log("Failed to create room.");
	}

	public override void OnJoinedRoom()
	{
		Log($"Joined room {roomCode.text}!");



		/*
		GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10)), Quaternion.identity);
		if (Random.Range(0, 2) == 1)
		{
			Mesh p2_1_inst = Instantiate(p2_1);
			player.GetComponent<MeshFilter>().mesh = p2_1_inst;
			player.GetComponent<MeshRenderer>().material = p2_1_mat;
		}
		*/
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.Log($"Failed to join room.\nError message: {returnCode}, {message}");


	}

	#endregion

	/// <summary>
	/// Sets the text of the log panel
	/// </summary>
	/// <param name="text">The text to add</param>
	private void Log(string text)
	{
		// TODO Make it more console like, messages don't overwrite but instead add themselves
		//logPanel.text = text;
		logPanel.SetText(text);
	}
}

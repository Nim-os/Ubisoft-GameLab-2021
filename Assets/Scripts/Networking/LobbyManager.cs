using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	private enum ConnectionState
	{
		Idle,
		Connecting,
		Joined,
		Host
	}

	public const string DevRoomID = "dev";

	public Button createButton;
	public Button joinButton;
	public Button playButton;
	public Button leaveButton;

	public TMP_InputField roomCodeInputField;

	public TextMeshProUGUI logPanel;
	public TextMeshProUGUI roomCodeText;

	private string roomCode = "";

	//public Mesh p2_1;
	//public Material p2_1_mat;

	#region Room Logic

	public void CreateRoom()
	{
		Log($"Creating room...");

		var room = new RoomOptions()
		{
			IsOpen = true,
			IsVisible = false,
			MaxPlayers = 2
		};

		roomCode = GetRandomCode().ToString();

		Debug.Log(roomCode);

		PhotonNetwork.CreateRoom(roomCode, room);
	}

	public void CreateDevRoom()
	{
		Log("Creating dev room...");

		var room = new RoomOptions()
		{
			IsOpen = true,
			IsVisible = false,
			MaxPlayers = 12
		};

		roomCode = DevRoomID;

		PhotonNetwork.CreateRoom(DevRoomID, room);
	}

	public void JoinRoom()
	{
		Log($"Joining room {roomCodeText.text}...");


		PhotonNetwork.JoinRoom(roomCodeText.text);
	}

	public void PlayGame()
	{
		Log("Starting game...");

		// TODO Players and min player checking
	}

	public void LeaveRoom()
	{
		Log("Left room.");

		// TODO
	}

	#endregion


	#region Callbacks

	public override void OnCreatedRoom()
	{
		roomCodeText.SetText(roomCode);

		Log($"Created room {roomCode} !");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		if (returnCode == 32766)
		{
			Debug.Log("Recreating room, created duplicate.");

			CreateRoom();
		}
		else
		{
			Debug.LogError($"Failed to create room.\n{returnCode}: {message}");
			Log("Failed to create room.");
		}
	}

	public override void OnJoinedRoom()
	{
		Log($"Joined room {roomCodeText.text} !");



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
		Debug.LogError($"Failed to join room.\n{returnCode}: {message}");

		switch (returnCode)
		{
			case 32758:
				Log("Room code not found.");

				break;

			case 32765:
				Log("Unable to join room, room is full.");

				break;

			default:
				Log("Could not join room.");
				break;
		}
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

	private short GetRandomCode()
	{
		short code = (short) Random.Range(0x0000, 0xFFFF);

		return code;
	}
}

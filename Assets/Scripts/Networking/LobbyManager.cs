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
		Started
	}

	public const string DevRoomID = "dev";

	public Button createButton;
	public Button joinButton;
	public Button playButton;
	public Button leaveButton;
	public Button devButton;

	public TMP_InputField inputField;

	public TextMeshProUGUI logPanel;

	private string roomCode = "";

	//public Mesh p2_1;
	//public Material p2_1_mat;

	private void Start()
	{
		Log("Connecting to game server...");
	}

	#region Room Logic

	public void CreateRoom()
	{
		Log($"Creating room...");

		SetConnectionState(ConnectionState.Connecting);

		var room = new RoomOptions()
		{
			IsOpen = true,
			IsVisible = true,
			MaxPlayers = 2
		};

		roomCode = GetRandomCode().ToString();

		PhotonNetwork.CreateRoom(roomCode, room);
	}

	public void JoinDevRoom()
	{
		Log("Creating dev room...");

		SetConnectionState(ConnectionState.Connecting);

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
		if (string.IsNullOrWhiteSpace(inputField.text))
		{
			Log("Must provide a room to join.");
		}
		else
		{
			Log($"Joining room {inputField.text}...");

			SetConnectionState(ConnectionState.Connecting);

			PhotonNetwork.JoinRoom(inputField.text);
		}
	}

	public void PlayGame()
	{
		Log("Starting game...");

		SetConnectionState(ConnectionState.Started);

		PhotonNetwork.LoadLevel(1); // TODO Change this to whatever scene, the number is the scene index in the build settings
	}

	public void LeaveRoom()
	{
		Log("Left room.");

		SetConnectionState(ConnectionState.Connecting);

		PhotonNetwork.LeaveRoom();
	}

	#endregion


	#region Callbacks

	public override void OnConnectedToMaster()
	{
		Log("Connected to the master server!");

		SetConnectionState(ConnectionState.Idle);
	}

	public override void OnCreatedRoom()
	{
		inputField.SetTextWithoutNotify(roomCode);

		Log($"Created room {roomCode} !");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		if (roomCode.Equals(DevRoomID))
		{
			PhotonNetwork.JoinRoom(roomCode);
		}
		else
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

				SetConnectionState(ConnectionState.Idle);
			}
		}
	}

	public override void OnJoinedRoom()
	{
		Log($"Joined room {roomCode} !");

		SetConnectionState(ConnectionState.Joined);


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
		switch (returnCode)
		{
			case 32758:
				Log("Room not found.");

				break;

			case 32765:
				Log("Unable to join room, room is full.");

				break;

			default:
				Debug.LogError($"Failed to join room.\n{returnCode}: {message}");

				Log("Unable to join room.");
				break;
		}

		SetConnectionState(ConnectionState.Idle);
	}

	public override void OnLeftRoom()
	{
		inputField.SetTextWithoutNotify("");
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		// TODO Make game unstartable
	}

	#endregion

	/// <summary>
	/// Sets the text of the log panel
	/// </summary>
	/// <param name="text">The text to add</param>
	private void Log(string text)
	{
		// TODO Make it more console like, messages don't overwrite but instead add themselves
		logPanel.SetText(text);
	}

	private ushort GetRandomCode()
	{
		ushort code = (ushort) Random.Range(0x2710, 0xFFFF);

		return code;
	}

	private void SetConnectionState(ConnectionState state)
	{
		switch (state)
		{
			case ConnectionState.Idle:
				inputField.interactable = true;

				createButton.interactable = true;
				joinButton.interactable = true;
				devButton.interactable = true;

				playButton.interactable = false;
				leaveButton.interactable = false;

				break;

			case ConnectionState.Connecting:
				inputField.interactable = false;

				createButton.interactable = false;
				joinButton.interactable = false;
				devButton.interactable = false;

				break;

			case ConnectionState.Joined:
				playButton.interactable = true;
				leaveButton.interactable = true;

				break;

			case ConnectionState.Started:
				playButton.interactable = false;
				leaveButton.interactable = false;

				break;
		}
	}
}

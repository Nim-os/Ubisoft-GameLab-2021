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

	/// <summary>
	/// Creates a random room.
	/// </summary>
	public void CreateRoom()
	{
		Log($"Creating room...");

		SetConnectionState(ConnectionState.Connecting);

		// Room options to designate our needs
		var room = new RoomOptions()
		{
			IsOpen = true,
			IsVisible = true,
			MaxPlayers = 2
		};

		roomCode = GetRandomCode().ToString();

		// Tell the network to create the room given the code and options
		PhotonNetwork.CreateRoom(roomCode, room);
	}

	/// <summary>
	/// Attempts to create the dev room and join it if it already exists.
	/// </summary>
	public void JoinDevRoom()
	{
		Log("Joining dev room...");

		SetConnectionState(ConnectionState.Connecting);

		// Dev room options to handle increased players
		var room = new RoomOptions()
		{
			IsOpen = true,
			IsVisible = false,
			MaxPlayers = 12
		};

		roomCode = DevRoomID;

		// Attempt to create the dev room
		PhotonNetwork.CreateRoom(DevRoomID, room);
	}

	/// <summary>
	/// Handles the player attempting to join a room.
	/// </summary>
	public void JoinRoom()
	{
		// Check if the input field is whitespace first so that we don't freak out Photon
		if (string.IsNullOrWhiteSpace(inputField.text))
		{
			Log("Must provide a room to join.");
		}
		else
		{
			Log($"Joining room {inputField.text}...");

			SetConnectionState(ConnectionState.Connecting);

			// Attempt to join the room if the input is valid
			PhotonNetwork.JoinRoom(inputField.text);
		}
	}

	/// <summary>
	/// Handles starting the game if player criterion is met.
	/// </summary>
	public void PlayGame()
	{
		Log("Starting game...");

		SetConnectionState(ConnectionState.Started);

		// Loads the initial level.
		PhotonNetwork.LoadLevel(1); // TODO Change this to whatever scene, the number is the scene index in the build settings
	}

	/// <summary>
	/// Handles the player leaving the room they are currently in.
	/// </summary>
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
		Log("Connected to game server.");

		SetConnectionState(ConnectionState.Idle);
	}

	public override void OnCreatedRoom()
	{
		// Set the input field to the room's code for clarity
		inputField.SetTextWithoutNotify(roomCode);

		Log($"Created room {roomCode} !");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		// Checks if we were trying to create a dev room
		if (roomCode.Equals(DevRoomID))
		{
			PhotonNetwork.JoinRoom(roomCode);
		}
		else
		{
			// Checks if the error code is related to using the code of an already existing room
			if (returnCode == 32766)
			{
				Debug.Log("Recreating room, created duplicate.");

				// Create a new room
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
		// Tries to match the error code to see if we can handle it
		switch (returnCode)
		{
			case 32758:
				Log("Room not found.");

				break;

			case 32765:
				Log("Unable to join room, room is full.");

				break;

			default:
				// Error code we aren't handling

				Debug.LogError($"Failed to join room.\n{returnCode}: {message}");

				Log("Unable to join room.");
				break;
		}

		SetConnectionState(ConnectionState.Idle);
	}

	public override void OnLeftRoom()
	{
		// Clears the input field whenever we leave a room
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
		logPanel.text += $"\n{text}";
	}

	/// <summary>
	/// Generates a random room code.
	/// </summary>
	/// <returns>A short on the interval [10000, 65535)</returns>
	private ushort GetRandomCode()
	{
		ushort code = (ushort) Random.Range(0x2710, 0xFFFF);

		return code;
	}

	/// <summary>
	/// Handles the interactability of UI input objects.
	/// </summary>
	/// <param name="state"></param>
	private void SetConnectionState(ConnectionState state)
	{
		// Handles all the fun button interactivity so that we don't kill Photon by trying to create a room whilst being in one already.
		// Nothing interesting here

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

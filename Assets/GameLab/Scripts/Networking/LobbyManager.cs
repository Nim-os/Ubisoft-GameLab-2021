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
	public Button messageButton;
	public Button backButton;
	public TMP_InputField inputField;

	public TextMeshProUGUI logPanel;
	[SerializeField]
	private AutoScroll autoScroll;

	[Header("DEBUG")]
	public bool forceStart = false;

	private string roomCode = "";
	private bool isHost = false;

	private void Start()
	{
		// Check if we are already connected
		if (!PhotonNetwork.IsConnected)
		{
			Log("Connecting to game server...");
		}
		// Check if we are already in a room
		else if (PhotonNetwork.InRoom)
		{
			// Set host
			isHost = PhotonNetwork.IsMasterClient;

			Log("Rejoined room.");

			if (isHost)
			{
				Log("You are the host.");
			}
			// Cycle through states not normally hit
			SetConnectionState(ConnectionState.Idle);
			SetConnectionState(ConnectionState.Connecting);
			SetConnectionState(ConnectionState.Joined);
			// Replace room code
			inputField.SetTextWithoutNotify(PhotonNetwork.CurrentRoom.Name);
		}
		else
		{
			Log("Connected to game server.");
			SetConnectionState(ConnectionState.Idle);
		}
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

			roomCode = inputField.text;

			// Attempt to join the room
			PhotonNetwork.JoinRoom(roomCode);

		}
	}

	/// <summary>
	/// Handles starting the game if player criterion is met.
	/// </summary>
	public void PlayGame()
	{
		// Check if we are the host
		if (!PhotonNetwork.IsMasterClient)
		{
			Log("Cannot start game, you are not the host.");
		}
		// Check if we meet conditions to start the game
		else if (PhotonNetwork.PlayerList.Length < 2 && !roomCode.Equals(DevRoomID) && !forceStart)
		{
			Log("Cannot start game, not enough players.");
		}
		// Start the game
		else
		{
			Log("Starting game...");

			SetConnectionState(ConnectionState.Started);


			// Loads the last stored level initialised to 2
			PhotonNetwork.LoadLevel(ServerManager.instance.lastLevel); // Currently set to scene 2: Tutorial
		}
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

	/// <summary>
	/// Brings player back to main menu and disconnects from photon
	/// </summary>
	public void BackToMain()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	/// <summary>
	/// Changes the name of the client player.
	/// </summary>
	/// <param name="name">The name to change to</param>
	public void ChangeName(string name)
	{
		PhotonNetwork.LocalPlayer.NickName = name;
	}

	/// <summary>
	/// Sends a console message to all players in a room.
	/// </summary>
	public void SendMessage()
	{
		photonView.RPC("RecieveMessage", RpcTarget.All, new object[] { PhotonNetwork.LocalPlayer, "Hi!" });
	}

	#endregion


	/// <summary>
	/// Recieves a message from other players and logs it to the console.
	/// </summary>
	/// <param name="player">The sender of the message</param>
	/// <param name="message">The message recieved</param>
	[PunRPC]
	private void RecieveMessage(Player player, string message)
	{
		Log($"{player.NickName}: {message}");
	}


	#region Callbacks

	public override void OnConnectedToMaster()
	{
		Log("Connected to game server.");

		SetConnectionState(ConnectionState.Idle);
		if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
	}

	public override void OnCreatedRoom()
	{
		// Set the input field to the room's code for clarity
		inputField.SetTextWithoutNotify(roomCode);

		isHost = true;

		Log($"Created room {roomCode}!");
		Log($"You are the host.");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		// Checks if we were trying to create a dev room
		if (roomCode.Equals(DevRoomID))
		{
			PhotonNetwork.JoinRoom(roomCode);
		}
		// Checks if the error code is related to using the code of an already existing room
		else if (returnCode == 32766)
		{
			Debug.Log("Recreating room, created duplicate.");

			// Create a new room
			CreateRoom();
		}
		// An unhandled case came up
		else
		{
			Debug.LogError($"Failed to create room.\n{returnCode}: {message}");

			Log("Failed to create room.");

			SetConnectionState(ConnectionState.Idle);
		}
	}

	public override void OnJoinedRoom()
	{
		if (!isHost)
		{
			Log($"Joined room {roomCode}!");
		}

		SetConnectionState(ConnectionState.Joined);
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
		isHost = false;

		SetConnectionState(ConnectionState.Idle);

		// Clears the input field whenever we leave a room
		inputField.SetTextWithoutNotify("");
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Log($"Player {newPlayer.NickName} joined.");
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Log($"Player {otherPlayer.NickName} left.");

		// Here so that we do not say that the client is the host if they were previously told that they were the host
		if (!isHost && PhotonNetwork.IsMasterClient)
		{
			isHost = true;
			playButton.interactable = true;

			Log("You are now the host.");
		}
	}

	#endregion

	/// <summary>
	/// Sets the text of the log panel
	/// </summary>
	/// <param name="text">The text to add</param>
	private void Log(string text)
	{
		logPanel.text += $"\n{text}";
		autoScroll.ScrollToBottom();
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
				backButton.interactable = true;

				playButton.interactable = false;
				leaveButton.interactable = false;
				messageButton.interactable = false;
				
				break;

			case ConnectionState.Connecting:
				inputField.interactable = false;

				createButton.interactable = false;
				joinButton.interactable = false;
				devButton.interactable = false;
				backButton.interactable = false;

				playButton.interactable = false;
				leaveButton.interactable = false;
				messageButton.interactable = false;

				break;

			case ConnectionState.Joined:
				if (isHost)
				{
					playButton.interactable = true;
				}
				
				leaveButton.interactable = true;
				messageButton.interactable = true;

				break;

			case ConnectionState.Started:
				playButton.interactable = false;
				leaveButton.interactable = false;
				messageButton.interactable = false;

				break;
		}
	}
}

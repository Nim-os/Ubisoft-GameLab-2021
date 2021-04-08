using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class ServerManager : MonoBehaviourPunCallbacks
{
	public enum Mode
	{
		Online, LocalSceneOnline, Offline
	}

	public static ServerManager instance;

	public Mode serverMode = Mode.Online;

	public int lastLevel = 2;

	void Awake()
	{
		// Keep ServerManager as an instance that carries over to multiple scenes
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(instance);
		}
		else
		{
			Destroy(this);
			return;
		}

		// If we want to run in offline mode without having to connect manually via a lobby	
		if (serverMode == Mode.Offline)
		{
			Debug.LogWarning("INFO: You are running in offline mode.");

			PhotonNetwork.OfflineMode = true;

			var room = new RoomOptions()
			{
				IsOpen = true,
				IsVisible = true,
				MaxPlayers = 2
			};

			PhotonNetwork.CreateRoom("offline", room);
		}
	}

	void Start()
	{
		if (serverMode != Mode.Offline)
		{
			Debug.Log("Attempting to connect to Server");

			// Might be able to create an AppSettings for connection and put this elsewhere
			PhotonNetwork.AutomaticallySyncScene = true;

			PhotonNetwork.ConnectUsingSettings();
		}
	}

	/// <summary>
	/// Gracefully disconnects from the Photon Network.
	/// </summary>
	public void Close()
	{
		PhotonNetwork.AutomaticallySyncScene = false;

		PhotonNetwork.LeaveRoom();

		PhotonNetwork.AutomaticallySyncScene = true;
	}

	#region Logic

	/// <summary>
	/// Loads the level of all players in the room.
	/// </summary>
	/// <remarks>
	/// If you want to return to the lobby scene, use RestartLevel() instead.
	/// </remarks>
	/// <param name="nextLevel">The desired level index</param>
	public void LoadRoomLevel(int nextLevel)
	{
		// Load level directly if we are the host
		if (PhotonNetwork.IsMasterClient)
		{
			HostLoadLevel(nextLevel);
		}
		// Load level 
		else
		{
			// Tell host to load the desired level
			photonView.RPC("HostLoadLevel", RpcTarget.MasterClient, new object[] { nextLevel });
		}
	}

	/// <summary>
	/// Loads the level.
	/// </summary>
	/// <param name="level">The level index</param>
	[PunRPC]
	private void HostLoadLevel(int level)
	{
		PhotonNetwork.LoadLevel(level);
	}

	/// <summary>
	/// Sends a message to all players to restart the level
	/// </summary>
	public void RestartLevel()
	{
		lastLevel = SceneManager.GetActiveScene().buildIndex;

		PhotonNetwork.LoadLevel(1);
	}

	public void KickAll()
	{
		Time.timeScale = 1f; // Edge case

		photonView.RPC("LeaveLevel", RpcTarget.Others);

		StartCoroutine(WaitToReturn());
	}

	private IEnumerator WaitToReturn()
	{
		// Wait for all other players to leave
		while (PhotonNetwork.PlayerList.Length > 1)
		{
			yield return new WaitForSeconds(0.15f);
		}

		LeaveLevel();
	}

	[PunRPC]
	private void LeaveLevel()
	{
		Time.timeScale = 1f; // Another edge case
		lastLevel = 2; // Reset lastLevel


		Close();

		SceneManager.LoadScene(0);
	}

	#endregion

	#region Callbacks

	public override void OnConnectedToMaster()
	{
		Debug.Log("Successfully connected to Photon server");

		// Create a private scene room if we are playing in the local scene
		if (serverMode == Mode.LocalSceneOnline)
		{
			// Create a special room

			var room = new RoomOptions()
			{
				IsOpen = true,
				IsVisible = true,
				MaxPlayers = 12
			};

			PhotonNetwork.JoinOrCreateRoom($"scene_{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}", room, TypedLobby.Default);
		}
	}

	public override void OnJoinedRoom()
	{
		// If we joined a private scene room, create our character
		if (serverMode == Mode.LocalSceneOnline)
		{
			PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
			PauseMenu.GameIsPaused=false;
			PauseMenu.disconnecting=false;
		}
	}


	public override void OnLeftRoom()
	{
		if (PhotonNetwork.IsConnectedAndReady)
		{
			base.OnLeftRoom();
			UnityEngine.SceneManagement.SceneManager.LoadScene(1);
		}
	}


	#endregion

}

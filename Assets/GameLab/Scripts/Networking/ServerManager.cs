using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks
{
	private static ServerManager instance;

	[Tooltip("Enable if you want to test offline without having to connect via a server.")]
	public bool offlineMode;

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
		if (offlineMode)
		{
			Debug.LogWarning("INFO: You are running in offline mode.");

			PhotonNetwork.OfflineMode = offlineMode;

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
		if (!offlineMode)
		{
			Debug.Log("Attempting to connect to Server");

			// Might be able to create an AppSettings for connection and put this elsewhere
			PhotonNetwork.AutomaticallySyncScene = true;

			PhotonNetwork.ConnectUsingSettings();
		}
	}

	#region Logic

	

	#endregion

	#region Callbacks

	public override void OnConnectedToMaster()
	{
		Debug.Log("Successfully connected to Photon server");
	}


	#endregion

}

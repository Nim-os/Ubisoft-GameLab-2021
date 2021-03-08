﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks
{
	public enum Mode
	{
		Online, LocalSceneOnline, Offline
	}

	public static ServerManager instance;

	public Mode serverMode = Mode.Online;

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

	#region Logic

	

	#endregion

	#region Callbacks

	public override void OnConnectedToMaster()
	{
		Debug.Log("Successfully connected to Photon server");

		if (serverMode == Mode.LocalSceneOnline)
		{
			var room = new RoomOptions()
			{
				IsOpen = true,
				IsVisible = true,
				MaxPlayers = 12
			};

			PhotonNetwork.JoinOrCreateRoom($"scene_{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}", room, TypedLobby.Default);
			Debug.Log($"scene_{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
		}
	}

	public override void OnJoinedRoom()
	{
		if (serverMode == Mode.LocalSceneOnline)
		{
			PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
		}
	}


	#endregion

}
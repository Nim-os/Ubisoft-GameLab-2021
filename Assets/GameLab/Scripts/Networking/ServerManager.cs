using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks
{
	private static ServerManager instance;

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
	}

	void Start()
	{
		Debug.Log("Attempting to connect to Server");

		// Might be able to create an AppSettings for connection and put this elsewhere
		PhotonNetwork.AutomaticallySyncScene = true;

		PhotonNetwork.ConnectUsingSettings();
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

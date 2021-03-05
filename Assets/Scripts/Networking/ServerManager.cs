using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks
{
	public static ServerManager instance;

	public bool isHost = false;

	public List<Player> players { get; private set; }

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

		players = new List<Player>();
	}

	void Start()
	{
		Debug.Log("Attempting to connect to Server");

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

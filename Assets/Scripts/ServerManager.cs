using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ServerManager : MonoBehaviourPunCallbacks
{
	void Start()
	{
		Debug.Log("Attempting to connect");

		PhotonNetwork.ConnectUsingSettings();
	}


	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Photon master server");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StageManager : MonoBehaviourPun
{
    public GameObject spawnsParent;

	private List<Transform> spawns;
	private bool[] chosenSpawns;
	private int spawnCount = 0;

	public Mesh p2_1;
	public Material p2_1_mat;

	private List<Player> players;

	private void Awake()
	{
		// Might be able to put this inside same if block as start since we don't need to do spawn positions
		// Better to just let one player handle them

		spawns = new List<Transform>(spawnsParent.transform.childCount);

		for (int i = 0; i < spawnsParent.transform.childCount; i++)
		{
			spawns.Add(spawnsParent.transform.GetChild(i));
		}

		chosenSpawns = new bool[spawns.Count];

		players = new List<Player>(PhotonNetwork.PlayerList);

		if (PhotonNetwork.IsMasterClient)
		{
			if (players.Count == 0)
			{
				Debug.LogError("No players in server manager. Did you try to run this scene directly? Use a lobby scene instead.");
			}
			else
			{
				SpawnPlayers();
			}
		}
	}

	private void SpawnPlayers()
	{		
		photonView.RPC("GeneratePlayer", RpcTarget.All, null);
	}

	[PunRPC]
	private void GeneratePlayer()
	{
		GameObject player = PhotonNetwork.Instantiate("Player", PickSpawnPosition(), Quaternion.identity);

		if (PhotonNetwork.IsMasterClient)
		{
			Mesh p2_1_inst = Instantiate(p2_1);
			player.GetComponent<MeshFilter>().mesh = p2_1_inst;
			player.GetComponent<MeshRenderer>().material = p2_1_mat;
		}
	}

	private Vector3 PickSpawnPosition()
	{
		int pos = Random.Range(0, spawns.Count);

		if (spawnCount < spawns.Count)
		{
			while (chosenSpawns[pos])
			{
				// Comment next line and uncomment next line if supporting many players and plenty of spawns.

				pos = Random.Range(0, spawns.Count);

				//pos = (pos + 1) % spawns.Count;
			}

			chosenSpawns[pos] = true;
			spawnCount += 1;
		}
		else
		{
			Debug.LogWarning("Not enough spawn positions for players.");
		}

		return spawns[pos].position;
	}
}

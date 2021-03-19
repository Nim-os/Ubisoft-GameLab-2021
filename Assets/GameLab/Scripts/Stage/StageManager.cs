using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
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

		// Retrieve all children of spawn gameobject that represent spawn points
		for (int i = 0; i < spawnsParent.transform.childCount; i++)
		{
			spawns.Add(spawnsParent.transform.GetChild(i));
		}

		chosenSpawns = new bool[spawns.Count];

		players = new List<Player>(PhotonNetwork.PlayerList);

	}

	private void Start()
	{
		if (!PhotonNetwork.InRoom)
		{
			Debug.LogError("Not inside a room. Did you run this scene correctly? Try running from the lobby scene instead.");

		}

		if (PhotonNetwork.IsMasterClient)
		{
			SpawnPlayers();
		}
	}

	private void SpawnPlayers()
	{
		// In case we are testing offline
		if (PhotonNetwork.OfflineMode)
		{
			GeneratePlayer(PickSpawnPosition());
		}
		else
		{
			// Indicates to each player to spawn their player character at a point
			foreach (Player player in players)
			{
				if (!player.IsLocal)
				{
					photonView.RPC("GeneratePlayer", player, new object[] { PickSpawnPosition() });
				}
			}

			GeneratePlayer(PickSpawnPosition());
		}
	}

	/// <summary>
	/// RPC Call that spawns a client owned player.
	/// </summary>
	/// <param name="spawnPos">Point to spawn at</param>
	[PunRPC]
	private void GeneratePlayer(Vector3 spawnPos)
	{
		Debug.Log("Generating a player");
		GameObject player = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

		// Issue with this is it only changes it client side
		// Workarounds are either create a new prefab for a second player or have all clients update the mesh
	}

	/// <summary>
	/// Function that picks a point based on the data of previous choices.
	/// Ensures that, given enough spawn points, all players will spawn in unique locations.
	/// </summary>
	/// <returns>The spawn point</returns>
	private Vector3 PickSpawnPosition()
	{
		// Get first potential position
		int pos = Random.Range(0, spawns.Count);

		// Check if there are even enough spawn points for players
		if (spawnCount < spawns.Count)
		{
			// While we haven't found an unoccupied spot
			while (chosenSpawns[pos])
			{
				pos = Random.Range(0, spawns.Count);
			}

			// Mark our chosen spot as taken
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

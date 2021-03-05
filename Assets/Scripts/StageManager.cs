using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StageManager : MonoBehaviour
{
    public GameObject spawnsParent;

	private List<Transform> spawns;

	public Mesh p2_1;
	public Material p2_1_mat;

	private void Awake()
	{
		// Might be able to put this inside same if block as start since we don't need to do spawn positions
		// Better to just let one player handle them

		spawns = new List<Transform>(spawnsParent.transform.childCount);

		for (int i = 0; i < spawnsParent.transform.childCount; i++)
		{
			spawns.Add(spawnsParent.transform.GetChild(i));
		}
	}

	private void Start()
	{
		if (ServerManager.instance.isHost)
		{
			if (ServerManager.instance.players.Count == 0)
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
		int index = 0;

		foreach (Player player in ServerManager.instance.players)
		{
			GameObject obj = GeneratePlayerGameObject(index);

			var view = obj.GetComponent<PhotonView>();

			view.TransferOwnership(player);

			index += 1;
		}
	}

	private GameObject GeneratePlayerGameObject(int index)
	{
		GameObject player = PhotonNetwork.Instantiate("Player", PickSpawnPosition(), Quaternion.identity);

		if (index % 2 == 0)
		{
			Mesh p2_1_inst = Instantiate(p2_1);
			player.GetComponent<MeshFilter>().mesh = p2_1_inst;
			player.GetComponent<MeshRenderer>().material = p2_1_mat;
		}

		return player;
	}

	private Vector3 PickSpawnPosition()
	{

		return Vector3.zero;
	}
}

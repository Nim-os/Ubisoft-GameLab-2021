using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject spawnsParent;

	private List<Transform> spawns;

	private void Awake()
	{
		spawns = new List<Transform>(spawnsParent.transform.childCount);

		for (int i = 0; i < spawnsParent.transform.childCount; i++)
		{
			spawns.Add(spawnsParent.transform.GetChild(i));
		}
	}

	private void Start()
	{
		SpawnPlayers();
	}

	private void SpawnPlayers()
	{

	}
}

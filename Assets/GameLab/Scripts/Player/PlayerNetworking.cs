using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworking : Photon.Pun.UtilityScripts.SmoothSyncMovement
{

	public Mesh host_mesh;
	public Material host_mat;

	private void Start()
	{
		// Set the skin of the host
		if (photonView.Owner.IsMasterClient)
		{
			//Mesh p2_1_inst = Instantiate(p2_1);
			GetComponent<MeshFilter>().mesh = host_mesh;
			GetComponent<MeshRenderer>().material = host_mat;
		}
	}
}

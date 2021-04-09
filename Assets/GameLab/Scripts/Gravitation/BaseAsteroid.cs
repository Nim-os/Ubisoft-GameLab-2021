using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BaseAsteroid : MonoBehaviourPun
{
    public float xthurst = 10f;
    public float zthrust = -15f;

    private Rigidbody rb;

    void Start()
    {
        
        // Set private variables
        rb = gameObject.GetComponent<Rigidbody>();

        Invoke("MyDestroy", 5);
    }

    private void Update()
    {
        rb.AddForce(new Vector3 (xthurst, 1, zthrust));
    }

    private void MyDestroy()
    {
        PhotonView pv = this.photonView;
        PhotonNetwork.Destroy(pv);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Keys : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") 
        PhotonNetwork.Destroy(this.gameObject);
    }
}

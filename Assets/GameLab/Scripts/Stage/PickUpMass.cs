using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickUpMass : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerPropulsion>().ChangeMass(rb.mass);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }    
}

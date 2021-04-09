﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Rocks : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerPropulsion pp = col.gameObject.GetComponent<PlayerPropulsion>();
            pp.ChangeMass(rb.mass);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}

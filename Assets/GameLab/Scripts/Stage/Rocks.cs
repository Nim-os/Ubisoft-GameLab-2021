﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Rocks : MonoBehaviourPun
{
    private Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerPropulsion pp = col.gameObject.GetComponent<PlayerPropulsion>();

            if (gameObject.GetComponent<PhotonView>().IsMine)
            {
                pp.ChangeMass(rb.mass);
            }

            Destroy(gameObject);
        }
    }
}

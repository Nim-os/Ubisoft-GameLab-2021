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
            col.gameObject.GetComponent<PlayerPropulsion>().ChangeMass(rb.mass);
            Destroy(this.gameObject);
        }
    }
}
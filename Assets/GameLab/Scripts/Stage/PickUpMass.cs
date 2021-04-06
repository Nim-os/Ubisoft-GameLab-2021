﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickUpMass : MonoBehaviour
{

    public GameObject text;
    private Rigidbody rb;
    private float respawnTime = 30;
    private MeshRenderer meshRenderer;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (text == null)
        {
            text = GameObject.FindGameObjectWithTag("GasNot");
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player" && meshRenderer.enabled)
        {
            PlayerPropulsion pp =  col.gameObject.GetComponent<PlayerPropulsion>();
            //Prevents player from becoming to large.
            if (pp.gas + rb.mass <= pp.startingGas) 
            {
                Debug.Log("Enough room");
                pp.ChangeMass(rb.mass);
                meshRenderer.enabled = false;
                StartCoroutine(WaitingCoroutine());
            } else {
                Debug.Log("Testing Function");
                StartCoroutine(ShowCantPickUp());
            }
        }
    }

    IEnumerator WaitingCoroutine()
    {
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        meshRenderer.enabled = true;
    }

    IEnumerator ShowCantPickUp()
    {
        Debug.Log("Function works");
        text.SetActive( true);
        yield return new WaitForSeconds(5);
        text.SetActive(false);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickUpMass : MonoBehaviour
{
    private Rigidbody rb;
    private float respawnTime = 30;
    private MeshRenderer meshRenderer;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player" && meshRenderer.enabled)
        {
            col.gameObject.GetComponent<PlayerPropulsion>().ChangeMass(rb.mass);
            meshRenderer.enabled = false;
            StartCoroutine(WaitingCoroutine());
        }
    }

    IEnumerator WaitingCoroutine()
    {
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        meshRenderer.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Keys : MonoBehaviour
{
    Rigidbody rb;
    GameObject stageManager;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        stageManager = GameObject.Find("StageManager");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") {
            PhotonNetwork.Destroy(this.gameObject);
            stageManager.GetComponent<LevelManager>().NewKey();
        }    
    }
}

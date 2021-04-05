using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour
{
    public GameObject endZone;
    public GameObject keyManager;
    public GameObject warpTime;
    private int ct;

    void Start()
    {
        warpTime.SetActive(false);
        endZone.SetActive(false);
        keyManager = GameObject.FindGameObjectWithTag("Key-m");
    }

    void Update()
    {
        ct = keyManager.transform.childCount;
        if (ct == 0) {
            warpTime.SetActive(true);
            endZone.SetActive(true);
        }
    }

}

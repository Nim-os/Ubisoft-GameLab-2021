using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour
{
    public GameObject endZone;
    public GameObject keyManager;
    public GameObject warpTime;
    public GameObject removeBorder;
    private int ct;
    private bool border_unlocked;
    private GameObject[] players;
    public GameObject win_canvas;

    void Start()
    {
        warpTime.SetActive(false);
        endZone.SetActive(false);
        keyManager = GameObject.FindGameObjectWithTag("Key-m");
        border_unlocked=false; //change 
        players=GameObject.FindGameObjectsWithTag("Player");
        //win_canvas=GameObject.Find("Win-Canvas");
    }

    void Update()
    {
        ct = keyManager.transform.childCount;
        if (ct ==0) { //change
            warpTime.SetActive(true);
            removeBorder.SetActive(false);
            endZone.SetActive(true);
            border_unlocked=true;
        }

        

        if(border_unlocked){
            bool both_crossed = true;
            foreach(GameObject i in players){
                if(i.transform.position.x<330){
                    both_crossed=false;
                }
            }

            if(both_crossed){
                win_canvas.SetActive(true);
            }
        }
    }

}

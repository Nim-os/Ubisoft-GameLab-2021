using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackHoleBehavious : MonoBehaviourPun
{
    public float forceAdd=0.00001f;
    public float swallowRange=2; //black hole swallow the game object if too close
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
            // Will now only work on objects tagged "Player" on scene.
            blkHoleAttract(); 
    }

    
    //function to add force to each rigidbody
    void blkHoleAttract(){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        for(int i=0;i<players.Length;i++)
        {
            GameObject player = players[i];
            if(Vector3.Distance(player.transform.position, transform.position) <= swallowRange)
            {   
                Destroy(player);
                i--;

            }else{
                    //add force to the object, but only for its x&z plane
                    player.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x-player.transform.position.x, 0, transform.position.z-player.transform.position.z)*forceAdd, ForceMode.Force);
            }
        }
    }
}

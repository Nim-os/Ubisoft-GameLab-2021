using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBehavious : MonoBehaviour
{
    List<Rigidbody> rbs ;
    public float forceAdd=0.00001f;
    public float swallowRange=2; //black hole swallow the game object if too close
    
    // Start is called before the first frame update
    void Start()
    {
        //list all the rbs 
        rbs=findRBInLayer(9);
    }

    // Update is called once per frame
    void Update()
    {
        if(rbs.Count!=0){
            blkHoleAttract();
        }
        
    }

    //helper methods to find all the game objects that has a certain layer number
    List<Rigidbody> findRBInLayer(int layer){
        //find all the game objects
        GameObject[] allObj=FindObjectsOfType<GameObject>();
        List<Rigidbody> allRB=new List<Rigidbody>();

        //check their layer
        for(int i=0;i<allObj.Length;i++){
            if(allObj[i].layer==layer){
                allRB.Add(allObj[i].GetComponent<Rigidbody>());
            }
        }

        //make sure it is null
        if(allRB.Count==0){
            return null;
        }

        return allRB;

    }

    
    //function to add force to each rigidbody
    void blkHoleAttract(){
        //make sure each rigidbody is on the same layer with player

        //Test without the player
        //TODO: include it when test it with everything
        //GameObject[] players=GameObject.FindGameObjectsWithTag("player");
        
        for(int i=0;i<rbs.Count;i++){
            Rigidbody current=rbs[i];
            //check whether current y is the same as the player

            //TODO: change it to player's y
            if(current.position.y==1){
                //ok to add force on current 

                if(Vector3.Distance(current.position, transform.position)<=swallowRange){
                    //destroy the gameobject
                    
                    Destroy(current.gameObject);
                    rbs.RemoveAt(i);
                    i--;

                }else{
                    //add force to the object, but only for its x&z plane
                    current.AddForce(new Vector3(transform.position.x-current.position.x, 0, transform.position.z-current.position.z)*forceAdd, ForceMode.Force);
                }

            }
        }
    }
}

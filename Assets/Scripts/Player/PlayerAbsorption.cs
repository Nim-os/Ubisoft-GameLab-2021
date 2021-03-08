using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbsorption : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    public float change_per_frame=0.1f; //change for the player mass and the absorbed object's mass per frame
    private Rigidbody absorbed_rb;//store the rb in collision
    private float total_mass=0;
    private PlayerPropulsion propulsionScript;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        propulsionScript = this.GetComponent<PlayerPropulsion>();
    }

    void Update(){
        //check whether there is unmerged object in process
        if(total_mass!=0){
            //mass left smaller than the change per frame
            if(total_mass<=change_per_frame){
                //update for both rbs
                ChangeMass(absorbed_rb, (float)-total_mass);
                propulsionScript.ChangeMass( (int)(total_mass*10));
                //reset total_mass
                total_mass=0;
                //destroy game object
                Destroy(absorbed_rb.gameObject);
                absorbed_rb=null;
            }else{
                //update rbs
                ChangeMass(absorbed_rb, (float)-change_per_frame);
                propulsionScript.ChangeMass((int)(change_per_frame*10));
                //update total_mass
                total_mass-=change_per_frame;
            }
            
        }
    }
    void OnCollisionEnter(Collision collision){
        
        if(total_mass==0){//make sure there is no absorbing in process
            bool isGameObj = collision.gameObject;
            bool hasRigidBody = collision.rigidbody;
            bool isGravitationalObj = (isGameObj) && (collision.gameObject.GetComponent<BaseGravitation>());
            bool hasLowerMass = (hasRigidBody) && (collision.rigidbody.mass < rb.mass);
        
            if (isGravitationalObj && hasLowerMass){
                float colliderMass = collision.rigidbody.mass;
                if (collision.gameObject.tag == "Player"){
                    print("hit player");
                }

                //Destroy(collision.gameObject);
                //propulsionScript.ChangeMass((int) colliderMass);

                total_mass=colliderMass;
                absorbed_rb=collision.rigidbody;
            }
        }
        
    }

    void ChangeMass(Rigidbody this_rb,float amount){
        //function for the absorbed object to change its mass
        this_rb.mass += amount;
        this_rb.gameObject.transform.localScale += new Vector3(amount,amount,amount);
    }
}

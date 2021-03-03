using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbsorption : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    int counter; //mimic the logarithmic growth
    public float startLogAt=0; //to start logarithmic growth once the size reach this number
    void Start()
    {
        rb=gameObject.GetComponent<Rigidbody>();
        counter=0;
    }

    void OnCollisionEnter(Collision collision){
        //increase the counter by 1
        

        //destroy the rigidbody
        float mass=0;
        if(collision.rigidbody){
            mass=collision.rigidbody.mass;
            Destroy(collision.rigidbody.gameObject);
        }

        //add the mass to the rigidbody
        //mimic logarithmic growth
        if(rb.mass>=startLogAt){
            //start log once the mass pass the threshold
            counter++;
            rb.mass+= mass*(1/counter);
        }else{
            rb.mass+= mass;
        }
        
        
        
    }
}

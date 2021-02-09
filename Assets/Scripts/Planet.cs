using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Rigidbody rb;
    public float affect_radius; //only attract planets within certain range
    

    // Start is called before the first frame update
    void Start()
    {
        //we can set mass and affect radius within ranges with random
        affect_radius=30;//default

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Implementing Newton's third law for gravitational potentials
    public void ThirdLaw(Planet attractor, float G)
    {
        Rigidbody opposite = attractor.rb;
        // Get direction of attraction
        Vector3 dir = rb.position - opposite.position;
        // Get magnitude
        float mag = dir.magnitude;
        
        // Third Law of Newtonian Mechanics (in Newtons)
        // Force scalar
        float forceMag = 100* (rb.mass * opposite.mass * G) / Mathf.Pow(mag,2);
        // Force vector
        Vector3 forceVect = -dir.normalized * forceMag;
        // Add force to rigidBody
        rb.AddForce(forceVect);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newton : MonoBehaviour
{
    /******* VARRIABLES *********/
    // Rigid body of object
    public Rigidbody rb;
    // G in SI units
    public float G = 6.67f * Mathf.Pow(10, 100);
    /******** METHODS ***********/
    // Fixed Update
    private void FixedUpdate()
    {
        // Creates array of bodies that can attract
        Newton[] bodies = FindObjectsOfType<Newton>();
        foreach(Newton body in bodies)
        {
            // Apply third law to each body (except this) in the array
            if (body != this) ThirdLaw(body);
        }
    }


    // Implementing Newton's third law for gravitational potentials
    void ThirdLaw(Newton attractor)
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

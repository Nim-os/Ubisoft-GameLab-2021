using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAsteroid : MonoBehaviour
{
    public int thurst;

    private Rigidbody rb;

    void Start(){
        // Set private variables
        rb = gameObject.GetComponent<Rigidbody>();

        // why doesnt lookat work
        rb.AddForce(Vector3.forward * thurst);
    }

}
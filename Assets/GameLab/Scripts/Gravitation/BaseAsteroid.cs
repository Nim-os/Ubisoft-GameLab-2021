using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAsteroid : MonoBehaviour
{
    public float xthurst;
    public float zthrust;

    private Rigidbody rb;

    void Start()
    {
        // Set private variables
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.AddForce(new Vector3 (xthurst, 1, zthrust));
    }
}

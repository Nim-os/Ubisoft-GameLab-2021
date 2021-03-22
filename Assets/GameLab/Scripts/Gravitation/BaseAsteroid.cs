using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAsteroid : MonoBehaviour
{
    public float xthurst = 10f;
    public float zthrust = -15f;

    private Rigidbody rb;

    void Start()
    {
        // Set private variables
        rb = gameObject.GetComponent<Rigidbody>();

        Destroy(this, 5f);
    }

    private void Update()
    {
        rb.AddForce(new Vector3 (xthurst, 1, zthrust));
    }
}

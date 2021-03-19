using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseRotation : MonoBehaviour
{
    public float speed;
    [SerializeField] private Rigidbody rb;

    void Start()
    {
        rb.angularVelocity = Random.insideUnitSphere * speed;
    }
}

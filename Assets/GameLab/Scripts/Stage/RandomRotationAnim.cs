using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationAnim : MonoBehaviour
{
    public float speed;
    [SerializeField] private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.angularVelocity = Random.insideUnitSphere * speed;
    }
}

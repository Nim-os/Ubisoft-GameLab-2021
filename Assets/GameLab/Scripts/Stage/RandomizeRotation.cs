using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeRotation : MonoBehaviour
{
    Animator animator;
    public float upperRandom = 100f;
    public float lowerRandom = 1f;
     
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        //animator.speed = Random.Range(lowerRandom,upperRandom);
        transform.localRotation = Random.rotation;
    }
}

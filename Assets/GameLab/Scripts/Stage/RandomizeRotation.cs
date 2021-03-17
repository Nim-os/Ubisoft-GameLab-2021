using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeRotation : MonoBehaviour
{
    Animator animator;
    public float upperRandom = 2f;
    public float lowerRandom = 0.08f;
     
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        animator.speed = Random.Range(lowerRandom,upperRandom);
        transform.localRotation = Random.rotation;
    }
}

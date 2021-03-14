using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxGame : MonoBehaviour
{
    private float spriteLength, spriteWidth;
    Vector3 startPosition;
    public GameObject cam;
    public float _parallaxEffect;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x/3;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.z/3;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 galilean = (cam.transform.position * (1 - _parallaxEffect));
        Vector3 distance = (cam.transform.position * _parallaxEffect);
        transform.position = new Vector3(startPosition.x + distance.x, transform.position.y, startPosition.z + distance.z);

        if (galilean.x > startPosition.x + spriteLength) startPosition.x += spriteLength;
        else if (galilean.x < startPosition.x - spriteLength) startPosition.x -= spriteLength;

        if (galilean.z > startPosition.z + spriteWidth) startPosition.z += spriteWidth;
        else if (galilean.z < startPosition.z - spriteWidth) startPosition.z -= spriteWidth;
    }
}

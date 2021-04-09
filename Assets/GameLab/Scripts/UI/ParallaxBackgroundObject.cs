using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundObject : MonoBehaviour
{
    [SerializeField]
    private float _width, _height;

    [SerializeField]
    private float _yValue;
    // Sprite size & position
    private float spriteLength, spriteWidth;
    Vector3 startPosition;
    // Camera
    public GameObject cam;
    // Effect parameter
    public float _parallaxEffect;


    private float yValue;

    // Start is called before the first frame update
    void Start()
    {
        yValue = cam.transform.position.y + _yValue;
        transform.eulerAngles = new Vector3(90, 0, 0);
        startPosition = cam.transform.position;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.z/2 ;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x/2 ;
    }

    // Update is called once per frame
    void Update()
    {
        // Frame of reference for parallax effect
        Vector3 galilean = (cam.transform.position * (1 - _parallaxEffect));
        // Frame of reference for observer
        Vector3 distance = (cam.transform.position * _parallaxEffect);
        // Galilean transform to compute parallax
        transform.position = new Vector3(startPosition.x + distance.x, _yValue, startPosition.z + distance.z);

        // Limits on galilean transform for parallax on x axis
        if (galilean.x > startPosition.x + spriteLength) startPosition.x += spriteLength;
        else if (galilean.x < startPosition.x - spriteLength) startPosition.x -= spriteLength;

        // Limits on galilean transform for parallax on z axis
        if (galilean.z > startPosition.z + spriteWidth) startPosition.z += spriteWidth;
        else if (galilean.z < startPosition.z - spriteWidth) startPosition.z -= spriteWidth;
    }
}

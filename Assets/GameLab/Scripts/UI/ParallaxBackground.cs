using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float _parallaxEffectX = .5f;
    public float _parallaxEffectZ = .5f;
    float textureUnitSizeX;
    float textureUnitSizeZ;

    Transform cam;
    Vector3 previous;
    private void Start()
    {
        cam = Camera.main.transform;
        previous = cam.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeZ = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 delta = cam.position - previous;
        transform.position += Vector3.right * delta.x * _parallaxEffectX + Vector3.forward * delta.z * _parallaxEffectZ;
        previous = cam.position;

        if (cam.position.x - transform.position.x >= textureUnitSizeX)
        {
            float offset = (cam.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = Vector3.right * (cam.position.x + offset) + Vector3.forward * transform.position.z;
        }

        if (cam.position.z - transform.position.z >= textureUnitSizeZ)
        {
            float offset = (cam.position.z - transform.position.z) % textureUnitSizeZ;
            transform.position = Vector3.right * transform.position.x + Vector3.forward * (cam.position.z + offset);
        }
    }
}
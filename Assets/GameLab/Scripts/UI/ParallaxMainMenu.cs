using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMainMenu : MonoBehaviour
{
    // Note: This parallax script works on a different plane than the 
    // script "ParallaxBackgroundObject", and thus is required to keep
    // separate
    
    // Value of parallax effect
    public float _parallaxEffect = .5f;
    
    // Texture unit size 
    float textureUnitSizeX;
    float textureUnitSizeZ;

    Transform cam;
    Vector3 previous;
    private void Start()
    {
        // Get previous camera position and current camera position
        cam = Camera.main.transform;
        previous = cam.position;

        // Initialize sprite unit sizes
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeZ = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        // Difference of current and previous cam position to give parallax effect.
        Vector3 delta = cam.position - previous;
        transform.position += Vector3.right * delta.x * _parallaxEffect + Vector3.forward * delta.z * _parallaxEffect;
        previous = cam.position;

        // Limits to repeat background on x axis
        if (cam.position.x - transform.position.x >= textureUnitSizeX)
        {
            float offset = (cam.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = Vector3.right * (cam.position.x + offset) + Vector3.forward * transform.position.z;
        }
        // Limits to repeat background on z axis
        if (cam.position.z - transform.position.z >= textureUnitSizeZ)
        {
            float offset = (cam.position.z - transform.position.z) % textureUnitSizeZ;
            transform.position = Vector3.right * transform.position.x + Vector3.forward * (cam.position.z + offset);
        }
    }
}

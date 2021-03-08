using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float _cameraSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.right * _cameraSpeed * Time.deltaTime;
    }
}

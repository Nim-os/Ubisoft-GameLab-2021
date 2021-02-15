using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    private BaseGravitation body;
    private Rigidbody rb;

    void Start(){
        // Set private variables
        body = transform.parent.gameObject.GetComponent<BaseGravitation>();
        rb = body.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        var hitObject = other.gameObject;
        // if gravitational object && not a planet
        if (hitObject.GetComponent<BaseGravitation>() && (hitObject.name != "BasicPlanet")){
            body.AddWithinRange(hitObject.GetComponent<BaseGravitation>());
        }
    }

    private void OnTriggerExit(Collider other) {
        var hitObject = other.gameObject;
        body.RemoveWithinRange(hitObject.GetComponent<BaseGravitation>());
    }
}

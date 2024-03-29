using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    [SerializeField]
    private BaseGravitation body;
    private Rigidbody rb;

    void Start(){
        // Set private variables
        body = transform.parent.gameObject.GetComponent<BaseGravitation>();
        rb = body.GetComponent<Rigidbody>();
    }

    // goal: figure out the problem with range & ontrigger enter and exit
    private void OnTriggerEnter(Collider other) {
        var hitObject = other.gameObject;
        // if gravitational object && not a planet
        if (hitObject.GetComponent<BaseGravitation>() && hitObject.GetComponent<BaseGravitation>().enabled && (hitObject.tag != "Planet" || hitObject.tag != "mass")){
            body.AddWithinRange(hitObject.GetComponent<BaseGravitation>());
        }
    }

    private void OnTriggerExit(Collider other) {
        var hitObject = other.gameObject;
        if (!hitObject.CompareTag("mass"))
            body.RemoveWithinRange(hitObject.GetComponent<BaseGravitation>());
    }
}

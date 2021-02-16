using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Basic gravitational script </summary>
public class BaseGravitation : MonoBehaviour
{
    public float G = 60f;
    public bool freezePosition = false;

    private List<BaseGravitation> ObjectsWithinRange = new List<BaseGravitation>();
    private Rigidbody rb;

    private void Start(){
        rb = gameObject.GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        // Updates force on objects that are within this object's range
        foreach (BaseGravitation o in ObjectsWithinRange){
            AttractMass(o);
        }
    }

    /// <summary> Add new object to ObjectsWithinRange list </summary>
    public void AddWithinRange(BaseGravitation collider){
        ObjectsWithinRange.Add(collider);
    }

    /// <summary> Remove object in ObjectsWithinRange list </summary>
    public void RemoveWithinRange(BaseGravitation collider){
        ObjectsWithinRange.Remove(collider);
    }

    /// <summary> Attract the other object towards this object </summary>
    private void AttractMass(BaseGravitation other){
        // Calculate magnitude and direction to apply force
        Rigidbody otherRb = other.rb;
        Vector3 dir = rb.position - otherRb.position;
        float mag = dir.magnitude;
        float forceMag = 100 * (rb.mass * otherRb.mass * G) / Mathf.Pow(mag,2);
        Vector3 forceVector = -dir.normalized * forceMag;

        // ACTIONS
        //rb.AddForce(forceVector); // this body goes towards other
        //otherRb.AddForce(forceVector); // other goes away from this
        otherRb.AddForce(-forceVector); // other goes towards this
        
        if (freezePosition){
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
    }
}

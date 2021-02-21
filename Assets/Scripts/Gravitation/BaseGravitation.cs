using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Basic gravitational script </summary>
public class BaseGravitation : MonoBehaviour
{
    public bool freezePosition = false;

    private float G = 1f;
    private List<BaseGravitation> ObjectsWithinRange = new List<BaseGravitation>();
    private Rigidbody rb;
    private bool isPlayer;

    private void Start(){
        rb = gameObject.GetComponent<Rigidbody>();
        isPlayer = gameObject.tag == "Player" ? true : false;
    }
    
    private void FixedUpdate()
    {
        // Updates force on objects that are within this object's range
        foreach (BaseGravitation o in ObjectsWithinRange){
            // if not a player
            // is a player && holding down right mouse button
            // attract

            if ((o != null) && (!(o.isPlayer) || (o.isPlayer && Input.GetMouseButton(1)))){
                AttractMass(o);
            }
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

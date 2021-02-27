using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Basic gravitational script </summary>
public class BaseGravitation : MonoBehaviour
{
    public bool freezePosition = false;
    public bool playerSelected = false;

    private float G = 1f;
    public List<BaseGravitation> ObjectsWithinRange = new List<BaseGravitation>();
    private Rigidbody rb;
    private bool isPlayer;

    private void Start(){
        rb = gameObject.GetComponent<Rigidbody>();
        isPlayer = gameObject.tag == "Player" ? true : false;
    }
    
    private void FixedUpdate()
    {
        for (int i = ObjectsWithinRange.Count-1; i >= 0; i--)
        {
            var o = ObjectsWithinRange[i];
            if ((o != null) && (!(o.isPlayer) || (o.isPlayer && Input.GetMouseButton(1) && playerSelected))){
                AttractMass(o);
            }else if (o == null){
                RemoveWithinRange(o);
            }
        }
    }

    /// <summary> Add new object to ObjectsWithinRange list </summary>
    public void AddWithinRange(BaseGravitation collider){
        //if new object is a player, add this object to player's gravity check list
        if (collider.gameObject.tag == "Player"){
            collider.gameObject.GetComponent<PlayerGravitation>().AddToGravityCheck(this);
        }
        ObjectsWithinRange.Add(collider);
    }

    /// <summary> Remove object in ObjectsWithinRange list </summary>
    public void RemoveWithinRange(BaseGravitation collider){
        if (collider && collider.gameObject.tag == "Player"){
            collider.gameObject.GetComponent<PlayerGravitation>().RemoveFromGravityCheck(this);
            playerSelected = false;
        }
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

    /*
    remove itself from any gravity lists
    */
    private void OnDestroy() {
        for (int i = ObjectsWithinRange.Count-1; i >= 0; i--)
        {
            var o = ObjectsWithinRange[i];
            
            // if one of the objects within range is a player
            if (o.isPlayer){
                // remove this object from its range
                o.GetComponent<PlayerGravitation>().RemoveFromGravityCheck(this.GetComponent<BaseGravitation>());
            }
            RemoveWithinRange(o);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Basic gravitational script </summary>
public class BaseGravitation : MonoBehaviour
{
    public InputSystem input;

    public bool freezePosition = false;
    public bool playerSelected = false;
    public bool beginRotating = false;

    private readonly float G = 1f;
    public List<BaseGravitation> ObjectsWithinRange = new List<BaseGravitation>();
    private Rigidbody rb;
    private bool isPlayer, holdingRMB = false;

	private void Awake()
	{
        input = new InputSystem();

        input.Game.Secondary.performed += x => holdingRMB = true;
        input.Game.Secondary.canceled += x => holdingRMB = false;
    }

	private void Start(){
        rb = gameObject.GetComponent<Rigidbody>();
        isPlayer = gameObject.tag == "Player" ? true : false;

        RigidbodyConstraints frozenPosition = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        RigidbodyConstraints frozenY = RigidbodyConstraints.FreezePositionY;

        rb.constraints = freezePosition ? frozenPosition : frozenY;
    }
    
    private void FixedUpdate()
    {
        for (int i = ObjectsWithinRange.Count-1; i >= 0; i--)
        {
            var o = ObjectsWithinRange[i];
            if ((o != null) && (!(o.isPlayer) || (o.isPlayer && holdingRMB && playerSelected))){
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

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}

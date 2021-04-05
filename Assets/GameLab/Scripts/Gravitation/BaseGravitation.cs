using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Basic gravitational script </summary>
public class BaseGravitation : MonoBehaviour
{
    public InputSystem input;

    public Shader selectionIndicator;

    public bool freezePosition = false;
    public bool playerSelected = false;
    public bool beginRotating = false;

    private readonly float G = 6.7f;
    public List<BaseGravitation> ObjectsWithinRange = new List<BaseGravitation>();
    private Rigidbody rb;
    private Renderer rend;
    private Shader defaultShader;
    private bool isPlayer, holdingRMB = false;

	private void Awake()
	{
        input = new InputSystem();

        input.Game.Secondary.performed += x => holdingRMB = true;
        input.Game.Secondary.canceled += x => holdingRMB = false;
    }

	private void Start(){
        rb = gameObject.GetComponent<Rigidbody>();
        rend = gameObject.GetComponent<Renderer>();

        defaultShader = rend.material.shader;

        isPlayer = gameObject.tag == "Player" ? true : false;

        StartCoroutine(PassiveStartRotation());
        
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
        // if this is a rock we don't want it caught in player's gravitation at launch
        if ((this.gameObject.CompareTag("Rock") && collider.gameObject.tag == "Player") ||
            (this.gameObject.CompareTag("Player") && collider.gameObject.tag == "Rock")
            ){
            return;
        }

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
                if (o != null) o.GetComponent<PlayerGravitation>().RemoveFromGravityCheck(this.GetComponent<BaseGravitation>());
            }
            RemoveWithinRange(o);
        }
    }

    IEnumerator PassiveStartRotation(){
        if (beginRotating){
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
       
            Vector3 force = new Vector3(Random.Range(50,100),Random.Range(50,100),Random.Range(50,100));
            rb.AddTorque(force*100);
        }
        
        yield return new WaitForSeconds(5);
    }

    /// <summary>
    /// Enables the shader to indicate gravitation
    /// </summary>
    public void ShowIndicator()
	{
        Debug.Log($"Started gravitating on {gameObject.name}");
        rend.material.shader = selectionIndicator;
	}

    /// <summary>
    /// Hides the shader to indicator no gravitation
    /// </summary>
    public void HideIndicator()
    {
        Debug.Log($"Stopped gravitating on {gameObject.name}");
        rend.material.shader = defaultShader;
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

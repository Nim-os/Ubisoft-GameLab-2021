using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public InputSystem input;

    public int holdingPower;
    public int rockSizeLimit;
    public bool holding;

    [SerializeField]
    private GameObject rockPrefab;    
    private float rockDespawnTime = 10;
    private PlayerPropulsion propulsionScript;
    private Vector2 mousePos = Vector2.zero;

    void Awake(){
        input = new InputSystem();

        input.Game.Projectile.performed += x => holding = true;
        input.Game.Projectile.canceled += x => holding = false;
        input.Game.MousePosition.performed += x => mousePos = x.ReadValue<Vector2>();
    }

    void Start(){
        propulsionScript = this.GetComponent<PlayerPropulsion>();
    }

    // Update is called once per frame
    void Update()
    {
        OnHold();
        OnLetGo();
    }

    /// <summary> When "holding", builds up size of projectile</summary>
    private void OnHold(){
        if (holding && (holdingPower < rockSizeLimit) && (propulsionScript.gas > 0)){
            holdingPower++;
        }
    }

    /// <summary> When "let go" releases projectile in mouse direction</summary>
    private void OnLetGo(){
        Vector3 mouseDir = Utils.GetMouseDirection(mousePos, gameObject);
        bool isLetGo = !holding;
        bool hasHoldingPower = holdingPower != 0;
        bool hasMouseDir = mouseDir != Vector3.zero;
        
        if (isLetGo && hasHoldingPower && hasMouseDir) {
            float playerSize = transform.localScale.x;
            propulsionScript.ChangeMass(-holdingPower);
            
            // create rock
            GameObject rock = Instantiate(rockPrefab, transform.position + mouseDir*(playerSize+2), transform.rotation);
            float length = holdingPower*0.05f;
            rock.transform.localScale = new Vector3(length, length, length);

            // apply force 
            rock.GetComponent<Rigidbody>().AddForce(mouseDir * holdingPower, ForceMode.Impulse);
            
            holdingPower = 0;
            Destroy(rock, rockDespawnTime);
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

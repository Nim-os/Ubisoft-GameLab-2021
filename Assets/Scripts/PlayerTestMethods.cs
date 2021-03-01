using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script with inputs for testing purposes </summary>
public class PlayerTestMethods : MonoBehaviour
{
    public InputSystem input;

    private Rigidbody rb;
    private int initGas;
    private float initMass;
    private Vector3 initScale;

    void Start()
    {
        if (!gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
		{
            Destroy(this);
            return;
		}

        rb = gameObject.GetComponent<Rigidbody>();
        initGas = this.GetComponent<PlayerPropulsion>().gas;
        initMass = rb.mass;
        initScale = this.transform.localScale;

        input = new InputSystem();

        
    }

    void FixedUpdate()
    {
        OnResetLocation();
        OnFillUpGas();
        OnGetMousePosition();

        if (rb.velocity.magnitude > 10) // What is this condition's purpose? Is it relevant to testing?
        {
            rb.velocity = Vector3.Normalize(rb.velocity) * 10;
        }
    }

    /// <summary> Reset location of player on "r".</summary>
    private void OnResetLocation(){
        if (Input.GetKeyDown("r"))
        {
            rb.velocity = Vector3.zero;
            this.transform.position = Vector3.zero;
        }
    }

    /// <summary> Reset location of player on "g".</summary>
    private void OnFillUpGas(){
        if (Input.GetKeyDown("g"))
        {
            gameObject.GetComponent<PlayerPropulsion>().gas = initGas;
            rb.mass = initMass;
            transform.localScale = initScale;
        }
    }

    /// <summary> Print out Vector3 mouse position</summary>
    private void OnGetMousePosition(){
        if (Input.GetKeyDown("y")){
            Debug.Log(Utils.mousePositionOnPlane());
        }
    }
}

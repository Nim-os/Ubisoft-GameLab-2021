using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Script with inputs for testing purposes </summary>
public class PlayerTestMethods : MonoBehaviour
{
    public InputSystem input;

    private Rigidbody rb;
    private Vector3 initScale;
    private GameObject gasbarUI;

    private Vector2 mousePos = Vector2.zero;

    private void Awake()
    {
        input = new InputSystem();

        // If we don't own this script, we can safely remove it to prevent other players from influencing the wrong player gameobject
        if (!gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
        {
            Destroy(this);
            return;
        }
        
        // Subscribe all relevant input

        input.Game.MousePosition.performed += x => mousePos = x.ReadValue<Vector2>();
        input.GameDebug.GasUp.performed += x => OnFillUpGas();
        input.GameDebug.LogPos.performed += x => OnGetMousePosition();
        input.GameDebug.ResetPos.performed += x => OnResetLocation();
    }


	void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        initScale = this.transform.localScale;
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > 10)
        {
            rb.velocity = Vector3.Normalize(rb.velocity) * 10;
        }

    }

    /// <summary> Reset location of player on "r".</summary>
    private void OnResetLocation()
    {
        rb.velocity = Vector3.zero;
        this.transform.position = Vector3.zero;
    }

    /// <summary> Reset gas of player on "g".</summary>
    private void OnFillUpGas()
    {
        gameObject.GetComponent<PlayerPropulsion>().ChangeMass(-1000);
        gameObject.GetComponent<PlayerPropulsion>().ChangeMass(30);
    }

    /// <summary> Print out Vector3 mouse position</summary>
    private void OnGetMousePosition()
    {
        Debug.Log(mousePos);
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

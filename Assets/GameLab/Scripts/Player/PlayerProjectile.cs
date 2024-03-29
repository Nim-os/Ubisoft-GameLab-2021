﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerProjectile : MonoBehaviourPunCallbacks
{
    public InputSystem input;

    public int holdingPower;
    public int rockSizeLimit;
    public bool holding = false;

    [SerializeField] private GameObject rockPrefab;    
    private float rockDespawnTime = 10;
    [SerializeField] private PlayerPropulsion propulsionScript;
    private Vector2 mousePos = Vector2.zero;
    private GameObject currentRock;
    private bool isMe;

    void Awake(){
        isMe = gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine;

        input = new InputSystem();

        if (isMe)
		{
            input.Game.Projectile.performed += x => holding = true;
            input.Game.Projectile.started += x => StartHold();
            input.Game.Projectile.canceled += x => OnLetGo();
            input.Game.MousePosition.performed += x => mousePos = x.ReadValue<Vector2>();
        }
    }

    void Start(){
        propulsionScript = this.GetComponent<PlayerPropulsion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMe && !PauseMenu.GameIsPaused)
        {
            OnHold();
        }
    }

    private void StartHold()
    {
        Vector3 mouseDir = Utils.GetMouseDirection(mousePos, gameObject);
        float playerSize = transform.localScale.x;

        if (propulsionScript.gas > 0){
            // create rock
            currentRock = PhotonNetwork.Instantiate(rockPrefab.name, transform.position + mouseDir*(playerSize+5), transform.rotation);
            currentRock.transform.localScale = new Vector3(1, 1, 1);
        } 
    }

    /// <summary> When "holding", builds up size of projectile</summary>
    private void OnHold()
    {
        Vector3 mouseDir = Utils.GetMouseDirection(mousePos, gameObject);
        float playerSize = transform.localScale.x;

        // holding and growing
        if (holding && (holdingPower < rockSizeLimit) && (propulsionScript.gas - holdingPower*0.1f > 0)){
            holdingPower++;
            float length = 1 + holdingPower*0.1f*0.2f;
            currentRock.transform.localScale = new Vector3(length, length, length);
            currentRock.transform.localPosition = transform.position + mouseDir*(playerSize+5);
        
        // holding but not growing
        }else if (holding && (holdingPower == rockSizeLimit)){
            currentRock.transform.localPosition = transform.position + mouseDir*(playerSize+5);
        }
    }

    /// <summary> When "let go" releases projectile in mouse direction</summary>
    private void OnLetGo()
    {
        Vector3 mouseDir = Utils.GetMouseDirection(mousePos, gameObject);
        bool hasHoldingPower = holdingPower != 0;
        bool hasMouseDir = mouseDir != Vector3.zero;
        holding = false;

        if (hasHoldingPower && hasMouseDir)
        {
            float playerSize = transform.localScale.x;
            propulsionScript.ChangeMass(-holdingPower*0.1f);
            
            float length = 1 + holdingPower*0.1f*0.2f;

            // rock rigidbody, apply force 
            Rigidbody rockRb = currentRock.GetComponent<Rigidbody>();
            rockRb.mass = length;
            rockRb.velocity = mouseDir*20;
            currentRock.GetComponent<BaseGravitation>().enabled = true;

            holdingPower = 0;
            Destroy(currentRock, rockDespawnTime);
        }
    }

    public override void OnEnable()
    {
        input.Enable();
    }

	public override void OnDisable()
	{
        input.Disable();
	}
}

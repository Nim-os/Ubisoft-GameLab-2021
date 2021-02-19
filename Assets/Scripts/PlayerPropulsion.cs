﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary> Contains all methods related to self propulsion </summary>
public class PlayerPropulsion : MonoBehaviour
{
    public InputSystem input;

    public float propulsionForce;
    public int gas;

    private Rigidbody rb;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    private bool isMe, propulsing;


	void Awake()
	{
        // Can we just destroy this script if it isn't the player? Message Christophe before removing this comment

        isMe = gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine;

        input = new InputSystem();

        if (isMe)
        {

            input.Game.Primary.performed += x => propulsing = true;
            input.Game.Primary.canceled += x => propulsing = false;

            input.Game.Reset.performed += x => OnResetLocation();
        }

    }

	void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        // Add player to cameraTargetGroup
        var cameraTargetGroup = GameObject.Find("CameraTargetGroup").GetComponent<CinemachineTargetGroup>();
        cameraTargetGroup.AddMember(this.transform,1,0);
    }

    void FixedUpdate()
    {
        if (propulsing)
		{
            OnPropulsion();
		}
        
    }

    /// <summary> On mouse button held down && not empty on gas, add force to player towards the mouse direction. </summary>
    private void OnPropulsion()
    {
        if (gas > 0)
        {
            // Use up gas when propulsion
            gas--;

            // Use mouse location to calculate direction to apply force
            var ray = Camera.main.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue());

            if (plane.Raycast(ray, out float enter))
            {
                var hitPoint = ray.GetPoint(enter);
                var mouseDirection = hitPoint - gameObject.transform.position;

                rb.AddForce(mouseDirection.normalized * propulsionForce, ForceMode.Impulse);
            }

        }
    }

    /// <summary> Reset location of player on "r". Delete later. </summary>
    private void OnResetLocation()
    {
        rb.velocity = Vector3.zero;
        this.transform.position = Vector3.zero;
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

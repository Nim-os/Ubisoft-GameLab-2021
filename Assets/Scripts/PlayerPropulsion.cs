using System.Collections;
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

    private bool isMe;

    private Vector3 force = Vector3.zero;

	void Awake()
	{
        input = new InputSystem();

        input.Game.Primary.performed += x => OnPropulsion();
        input.Game.Reset.performed += x => OnResetLocation();

        isMe = gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine;
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
        if (isMe && force != Vector3.zero)
		{
            rb.AddForce(force, ForceMode.Impulse);

            force = Vector3.zero;
		}
        
    }

    /// <summary> On mouse button held down && not empty on gas, add force to player towards the mouse direction </summary>
    private void OnPropulsion()
    {
        if (isMe && gas > 0)
        {
            // Use up gas when propulsion
            gas--;

            // Use mouse location to calculate direction to apply force
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float enter))
            {
                var hitPoint = ray.GetPoint(enter);
                var mouseDirection = hitPoint - gameObject.transform.position;
                mouseDirection = mouseDirection.normalized;

                force = mouseDirection * propulsionForce;
            }
        }
    }

    /// <summary> Reset location of player on "r". Delete later. </summary>
    private void OnResetLocation()
    {
        if (isMe)
        {
            rb.velocity = Vector3.zero;
            this.transform.position = Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script with inputs for testing purposes </summary>
public class PlayerTestMethods : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
		{
            OnResetLocation();
            OnFillUpGas();
            OnGetMousePosition();
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
            gameObject.GetComponent<PlayerPropulsion>().gas = 100;
        }
    }

    /// <summary> Print out Vector3 mouse position</summary>
    private void OnGetMousePosition(){
        if (Input.GetKeyDown("y")){
            Debug.Log(Utils.mousePositionOnPlane());
        }
    }
}

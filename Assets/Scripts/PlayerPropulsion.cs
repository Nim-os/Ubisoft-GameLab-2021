using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary> Contains all methods related to self propulsion </summary>
public class PlayerPropulsion : MonoBehaviour
{
    public float propulsionForce;
    public int gas;

    private Rigidbody rb;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        // Add player to cameraTargetGroup
        var cameraTargetGroup = GameObject.Find("CameraTargetGroup").GetComponent<CinemachineTargetGroup>();
        cameraTargetGroup.AddMember(this.transform,1,0);
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
		{
            OnPropulsion();
            OnResetLocation();
        }
    }

    /// <summary> On mouse button held down && not empty on gas, add force to player towards the mouse direction </summary>
    private void OnPropulsion(){
        if (Input.GetMouseButton(0) && gas > 0)
        {
            // Use up gas when propulsion
            gas--;

            ApplyForceInMouseDirection();
            RemoveVolume();
        }else if (Input.GetMouseButton(0) && gas < 1){
            Debug.Log("ran out of gas");
        }
    }

    private void OnCollisionEnter(Collision other) {
        var thisLocalScale = gameObject.transform.localScale;
        var thisVolume = thisLocalScale.x * thisLocalScale.y * thisLocalScale.z;
        var otherLocalScale = other.gameObject.transform.localScale;
        var otherVolume = otherLocalScale.x * otherLocalScale.y * otherLocalScale.z;

        // if not another Player && smaller
        if ((other.gameObject.tag != "Player") && (otherVolume <= thisVolume)){
            // destroy eaten object
            Destroy(other.gameObject);
            // add its volume to the player
            AddVolume(otherLocalScale);
        }
    }

    /// <summary> Remove volume from player </summary>
    void RemoveVolume(){
        //rb.mass--;
        gameObject.transform.localScale -= new Vector3(0.001f,0.001f,0.001f);
    }

    /// <summary> Add volume to player </summary>
    private void AddVolume(Vector3 volumeAddBy){
        // add more gas
        gas += (int) volumeAddBy.x;

        // TODO: change to smoother animation
        gameObject.transform.localScale += volumeAddBy;
    }

    /// <summary> Use mouse position to calculate direction and apply force </summary>
    private void ApplyForceInMouseDirection(){
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter;
        if (plane.Raycast(ray, out enter))
        {
            var hitPoint = ray.GetPoint(enter);
            var mouseDirection = hitPoint - gameObject.transform.position;
            mouseDirection = mouseDirection.normalized;
            rb.AddForce(mouseDirection * propulsionForce, ForceMode.Impulse);
        }
    }

    /// <summary> Reset location of player on "r". Delete later. </summary>
    private void OnResetLocation(){
        if (Input.GetKeyDown("r"))
        {
            rb.velocity = Vector3.zero;
            this.transform.position = Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary> Contains all methods related to self propulsion </summary>
public class PlayerPropulsion : MonoBehaviour
{
    public float propulsionForce;
    public int gas;
    public int holdingPower;

    [SerializeField]
    private GameObject rockPrefab;
    private Rigidbody rb;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private CinemachineTransposer cameraTransposer;
    private float cameraHeight;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cameraTransposer = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        cameraHeight = cameraTransposer.m_FollowOffset.y;

        // Add player to cameraTargetGroup
        var cameraTargetGroup = GameObject.Find("CameraTargetGroup").GetComponent<CinemachineTargetGroup>();
        cameraTargetGroup.AddMember(this.transform,1,0);
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
		{
            // Hold down mouse button to build up force, let go to launch
            OnHold();
            OnLetGo();

            // Test input methods
            TestInputs();
        }
    }

    // rename this method better
    /// <summary> When "holding", builds up force for the player to use when they let go </summary>
    private void OnHold(){
        if (Input.GetMouseButton(0) && gas > 0){
            holdingPower++;
            gas--;
        }
    }

    // rename this method better
    /// <summary> When "let go" applies force to player in mouse direction according to force built up & creates a rock and applies force on it in the opposite direction of the mouse</summary>
    private void OnLetGo(){
        Vector3 mouseDir;
        if (!Input.GetMouseButton(0) && holdingPower != 0 && ((mouseDir = GetMouseDirection()) != Vector3.zero)){
            // apply force on the player
            rb.AddForce(mouseDir * propulsionForce * holdingPower, ForceMode.Impulse);

            // create rock
            GameObject rock = Instantiate(rockPrefab, transform.position + -mouseDir*2, transform.rotation);
            float length = holdingPower*0.05f;
            rock.transform.localScale = new Vector3(length, length, length);

            // apply force 
            rock.GetComponent<Rigidbody>().AddForce(-mouseDir * propulsionForce * holdingPower, ForceMode.Impulse);
            
            holdingPower = 0;
        }
    }

    /// <summary> Gets mouse direction </summary>
    private Vector3 GetMouseDirection(){
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter;

        if (plane.Raycast(ray, out enter))
        {
            var hitPoint = ray.GetPoint(enter);
            var mouseDirection = hitPoint - gameObject.transform.position;
            mouseDirection = mouseDirection.normalized;
            return mouseDirection;
        }
        // did not hit
        return Vector3.zero;
    }

    /// <summary> On mouse button held down && not empty on gas, add force to player towards the mouse direction </summary>
    private void OnPropulsion(){
        if (Input.GetMouseButton(0) && gas > 0)
        {
            // move camera height, commented out for now to be less annoying when testing/developing base gameplay
            // cameraHeight = cameraHeight - (float) 0.05;
            // SetCameraHeight(cameraHeight);

            // Use up gas when propulsion
            gas--;

            // Use mouse location to calculate direction to apply force
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
    }

    private void SetCameraHeight(float height){
        cameraTransposer.m_FollowOffset.y = height;
    }

    /// <summary> Reset location of player on "r". Delete later. </summary>
    private void OnResetLocation(){
        if (Input.GetKeyDown("r"))
        {
            rb.velocity = Vector3.zero;
            this.transform.position = Vector3.zero;
        }
    }

    /// <summary> Reset location of player on "g". Delete later. </summary>
    private void OnFillUpGas(){
        if (Input.GetKeyDown("g"))
        {
            gas = 100;
        }
    }

    private void TestInputs(){
        OnResetLocation();
        OnFillUpGas();
    }
}

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
    private float rockDespawnTime = 10;
    private Rigidbody rb;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private CinemachineTransposer cameraTransposer;
    private float cameraHeight;
    private ParticleSystem propulsionParticles;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cameraTransposer = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        cameraHeight = cameraTransposer.m_FollowOffset.y;
        propulsionParticles = this.GetComponent<ParticleSystem>();

        // Add player to cameraTargetGroup
        var cameraTargetGroup = GameObject.Find("CameraTargetGroup").GetComponent<CinemachineTargetGroup>();
        cameraTargetGroup.AddMember(this.transform,1,0);
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
		{
            StartParticles(); 
            OnPropulsion();
            StopParticles();
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
        
        if (!Input.GetMouseButton(0) && holdingPower != 0 && ((mouseDir = Utils.GetMouseDirection(gameObject)) != Vector3.zero)){
            // apply force on the player
            rb.AddForce(mouseDir * propulsionForce * holdingPower, ForceMode.Impulse);

            // create rock
            GameObject rock = Instantiate(rockPrefab, transform.position + -mouseDir*2, transform.rotation);
            float length = holdingPower*0.05f;
            rock.transform.localScale = new Vector3(length, length, length);

            // apply force 
            rock.GetComponent<Rigidbody>().AddForce(-mouseDir * propulsionForce * holdingPower, ForceMode.Impulse);
            
            holdingPower = 0;

            Destroy(rock, rockDespawnTime);
        }
    }

    /// <summary> On mouse button held down && not empty on gas, add force to player towards the mouse direction </summary>
    private void OnPropulsion(){
        if (Input.GetMouseButton(0) && gas > 0)
        {
            // Use up gas when propulsion
            gas--;
            rb.mass -= 0.01f;
            transform.localScale -= new Vector3(0.01f,0.01f,0.01f);
            
            Vector3 mouseDirection = Utils.GetMouseDirection(gameObject);
            rb.AddForce(mouseDirection * propulsionForce, ForceMode.Impulse);
        }
        
    }
    
    private void StartParticles(){
        if (Input.GetMouseButtonDown(0) && gas > 0){
            propulsionParticles.Play();
        }
    }
    
    private void StopParticles(){
        if (Input.GetMouseButtonUp(0) && gas > 0){
            propulsionParticles.Stop();
        }
    }
}

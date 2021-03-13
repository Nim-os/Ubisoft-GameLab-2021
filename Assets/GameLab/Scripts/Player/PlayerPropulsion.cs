using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

/// <summary> Contains all methods related to self propulsion </summary>
public class PlayerPropulsion : MonoBehaviour
{
    public InputSystem input;
    public float gasUIMultiplier = 0.002f;
    public float massMultiplier = 0.01f;
    public float scaleMultiplier = 0.01f;

    public float propulsionForce;
    public float gas;
    public int holdingPower;

    [SerializeField]
    private GameObject rockPrefab;
    private float rockDespawnTime = 10;
    private Rigidbody rb;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private CinemachineTransposer cameraTransposer;
    private float cameraHeight;
    private ParticleSystem propulsionParticles;
    private Vector2 mousePos = Vector2.zero;
    private bool propulsing = false;
    private Image gasBar;

	void Awake()
    {
        input = new InputSystem();

        // If we don't own this script, we can safely remove it to prevent other players from influencing the wrong player gameobject
        if (!gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
        {
            Destroy(this);
            return;
        }

        input.Game.Primary.started += x => StartParticles();
        input.Game.Primary.performed += x => propulsing = true;
        input.Game.Primary.canceled += x => StopParticles();
        
        input.Game.MousePosition.performed += x => mousePos = x.ReadValue<Vector2>();
    }

	void Start()
    {
        gasBar = GameObject.Find("GasBarUI").GetComponent<Image>();
        rb = gameObject.GetComponent<Rigidbody>();
        cameraTransposer = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        cameraHeight = cameraTransposer.m_FollowOffset.y;
        propulsionParticles = this.GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (propulsing)
		{
            OnPropulsion();
        }
    }

    // rename this method better
    /// <summary> When "holding", builds up force for the player to use when they let go </summary>
    private void OnHold(){
        if (propulsing && gas > 0){
            holdingPower++;
            gas--;
        }
    }

    // rename this method better
    /// <summary> When "let go" applies force to player in mouse direction according to force built up & creates a rock and applies force on it in the opposite direction of the mouse</summary>
    private void OnLetGo(){
        Vector3 mouseDir;
        
        if (!propulsing && holdingPower != 0 && ((mouseDir = Utils.GetMouseDirection(mousePos, gameObject)) != Vector3.zero)){
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

    /// <summary> On mouse button held down && not empty on gas, add force to player towards the mouse direction. </summary>
    private void OnPropulsion()
    {
        if (gas > 0)
        {
            // Use up gas when propulsion
            ChangeMass(-1);

            Vector3 mouseDirection = Utils.GetMouseDirection(mousePos, gameObject);
            rb.AddForce(mouseDirection * propulsionForce, ForceMode.Impulse);
        }else{
            StopParticles();
        }
    }

    /// <summary>
	/// Changes the player's mass
	/// </summary>
	/// <param name="amount">The amount to add</param>
    public void ChangeMass(float amount){
        gas += amount;
        rb.mass += amount * massMultiplier;
        transform.localScale += new Vector3(scaleMultiplier*amount,scaleMultiplier*amount,scaleMultiplier*amount);
        gasBar.fillAmount += (float) amount * gasUIMultiplier;
    }
    
    /// <summary>
	/// Begins player's particle effects on propulsion
	/// </summary>
    private void StartParticles(){
        if (gas > 0){
            propulsing = true;
            propulsionParticles.Play();
        }
    }
    
    /// <summary>
	/// Ends player's particle effects on stop propulsion
	/// </summary>
    private void StopParticles(){
        propulsing = false;
        propulsionParticles.Stop();
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

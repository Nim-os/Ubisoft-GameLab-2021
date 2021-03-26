using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary> Contains all methods related to self propulsion </summary>
public class PlayerPropulsion : MonoBehaviour
{
    public InputSystem input;

    public float propulsionForce;
    public float gas;
    public int holdingPower;

    [SerializeField] private GameObject rockPrefab;
    private float rockDespawnTime = 10;
    [SerializeField] private Rigidbody rb;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private CinemachineTransposer cameraTransposer;
    private float cameraHeight;
    private ParticleSystem propulsionParticles;
    private Vector2 mousePos = Vector2.zero;
    private bool propulsing = false, particlesEnabled = false, particlesLastState = false;
    private Image gasBar;

    private PhotonView photonView;
    private float particleRPCDecay = 0.33f;

	void Awake()
    {
        input = new InputSystem();

        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            input.Game.Primary.started += x => StartParticles();
            input.Game.Primary.performed += x => propulsing = true;
            input.Game.Primary.canceled += x => StopParticles();

            input.Game.MousePosition.performed += x => mousePos = x.ReadValue<Vector2>();
        }
    }

	void Start()
    {
        gasBar = GameObject.Find("GasBarUI").GetComponent<Image>();
        cameraTransposer = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        cameraHeight = cameraTransposer.m_FollowOffset.y;
        propulsionParticles = this.GetComponent<ParticleSystem>();
    }

	private void Update()
	{
        particleRPCDecay -= Time.deltaTime;

        // Check if we should update other players on if we are propulsing or not visually
        if (particleRPCDecay < 0 && particlesEnabled != particlesLastState)
		{
            // Reset RPC decay
            particleRPCDecay = 0.33f;

            // Set the new state of the particles to save ourselves from multiple similar RPC calls
            particlesLastState = particlesEnabled;

            // Send RPC to other players
            photonView.RPC("ParticlesRPC", RpcTarget.Others, new object[] { particlesEnabled });
		}
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
    private void OnHold()
    {
        if (propulsing && gas > 0){
            holdingPower++;
            gas--;
        }
    }

    // rename this method better
    /// <summary> When "let go" applies force to player in mouse direction according to force built up & creates a rock and applies force on it in the opposite direction of the mouse</summary>
    private void OnLetGo()
    {
        Vector3 mouseDir;
        
        if (!propulsing && holdingPower != 0 && ((mouseDir = Utils.GetMouseDirection(mousePos, gameObject)) != Vector3.zero)){
            // apply force on the player
            rb.AddForce(mouseDir * propulsionForce * holdingPower, ForceMode.Impulse);

            // create rock
            GameObject rock = PhotonNetwork.Instantiate(rockPrefab.name, transform.position + -mouseDir*2, transform.rotation);
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
            ChangeMass(-0.002f);

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
    public void ChangeMass(float amount)
    {
        float maxWidth = 400f;

        // adjust gas
        gas += amount;

        // adjust other values based off of gas
        float newScale = 1 + (gas * (1/5));
        transform.localScale = new Vector3(newScale, newScale, newScale);
        gasBar.fillAmount = maxWidth * (((float) gas)/100f);
        rb.mass = newScale;
    }
    
    /// <summary>
	/// Begins player's particle effects on propulsion
	/// </summary>
    private void StartParticles()
    {
        if (gas > 0)
        {
            propulsing = true;
            particlesEnabled = true;
            propulsionParticles.Play();
        }
    }
    
    /// <summary>
	/// Ends player's particle effects on stop propulsion
	/// </summary>
    private void StopParticles()
    {
        propulsing = false;
        particlesEnabled = false;
        propulsionParticles.Stop();
    }

    /// <summary>
    /// Update the state of particles on this Player's gameobject.
    /// </summary>
    /// <param name="enabled">true if we should enable particles</param>
    [PunRPC]
    public void ParticlesRPC(bool enabled)
	{
        if (enabled)
        {
            propulsionParticles.Play();
        }
        else
        {
            propulsionParticles.Stop();
        }
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

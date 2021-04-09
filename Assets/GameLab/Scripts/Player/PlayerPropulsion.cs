using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary> Contains all methods related to self propulsion </summary>
public class PlayerPropulsion : MonoBehaviourPunCallbacks
{

    [SerializeField]
    public GameObject gasBarPrefab;
    public InputSystem input;
    public float propulsionForce;
    [Range(0, 100)]
    public float startingGas = 30;
    public float gas { get;private set; }
    public int holdingPower;
    public float MassLossRatio 
    {
        get {return _massLossRatio;}
        set {_massLossRatio = Mathf.Clamp(_massLossRatio, 0, 1);}
    }
    [SerializeField]
    private float _massLossRatio;
    [SerializeField] private GameObject rockPrefab;
    private float rockDespawnTime = 10;
    [SerializeField] private Rigidbody rb;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private CinemachineTransposer cameraTransposer;
    private float cameraHeight;
    private ParticleSystem propulsionParticles;
    private Vector2 mousePos = Vector2.zero;
    private bool propulsing = false, particlesEnabled = false, particlesLastState = false;
    // Changed to public to allow "GasBarUI.cs" to access photonView;
    // Wouldn't work otherwise.
    private float particleRPCDecay = 0.33f;

	void Awake()
    {
        input = new InputSystem();

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
        if (gasBarPrefab != null && PhotonNetwork.LocalPlayer.Equals(photonView.Owner))
        {
            GameObject _uiGB = Instantiate(this.gasBarPrefab);
            _uiGB.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        
        else
            Debug.LogError("<Color=Red><a>Missing</a></Color> gasBarPrefab reference on player Prefab.", this);

        cameraTransposer = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        cameraHeight = cameraTransposer.m_FollowOffset.y;
        propulsionParticles = this.GetComponent<ParticleSystem>();
        gas = startingGas;
        ChangeMass(0);
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
        if (propulsing && !PauseMenu.GameIsPaused)
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
            gas=gas - 1*_massLossRatio;
            // if _massLossRatio == 0 then no mass lost during propulsion (GOOD FOR TESTING)
            // if _massLossRatio = 1, Initial setting pre _massLossRatio
            // Currently set to 0.33
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
            ChangeMass(-0.1f * _massLossRatio);

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
        if (PhotonNetwork.LocalPlayer.Equals(photonView.Owner))
		{
            ChangeMassNetwork(amount);
		}
        else
        {
            photonView.RPC("ChangeMassNetwork", photonView.Owner, new object[] { amount });
        }
    }

    /// <summary>
    /// Changes the mass of the player. This is only meant to be called by RPC
    /// </summary>
    /// <param name="amount">Amount to add</param>
    [PunRPC]
    private void ChangeMassNetwork(float amount)
    {
        // adjust gas
        gas += amount;

        // if gets below 0, sets to 0
        gas = gas < 0 ? 0 : gas;

        // adjust other values based off of gas
        float newScale = gas * 0.2f + 1;
        transform.localScale = new Vector3(newScale, newScale, newScale);
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

    void CalledOnLevelWasLoaded()
    {
        GameObject _uiGB = Instantiate(this.gasBarPrefab);
        _uiGB.SendMessage("Set Target", this, SendMessageOptions.RequireReceiver);
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

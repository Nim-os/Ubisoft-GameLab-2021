using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class PlayerAbsorption : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private PlayerPropulsion propulsionScript;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        propulsionScript = this.GetComponent<PlayerPropulsion>();
    }

    void OnCollisionEnter(Collision collision){

        bool hasRigidBody = collision.rigidbody;
        bool isGravitationalObj = (collision.gameObject.GetComponent<BaseGravitation>());
      

        if (isGravitationalObj && collision.gameObject.tag == "mass")
        {
            float colliderMass = collision.rigidbody.mass;
            if (collision.gameObject.CompareTag("Player"))
            {
                //SceneManager.LoadScene(0);
                return;
            }

            PhotonNetwork.Destroy(collision.gameObject);
            propulsionScript.ChangeMass(colliderMass);
        }
    }
}

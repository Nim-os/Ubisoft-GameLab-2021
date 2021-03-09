using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbsorption : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    private PlayerPropulsion propulsionScript;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        propulsionScript = this.GetComponent<PlayerPropulsion>();
    }

    void OnCollisionEnter(Collision collision){
        bool hasRigidBody = collision.rigidbody;
        bool isGravitationalObj = (collision.gameObject.GetComponent<BaseGravitation>());
        bool hasLowerMass = (hasRigidBody) && (collision.rigidbody.mass < rb.mass);
        
        if (isGravitationalObj && hasLowerMass){
            float colliderMass = collision.rigidbody.mass;
            if (collision.gameObject.CompareTag("Player")){
                print("hit player");
            }

            Destroy(collision.gameObject);
            propulsionScript.ChangeMass((colliderMass - 1)/0.01f);
        }
    }
}

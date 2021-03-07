using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbsorption : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    private PlayerPropulsion propulsionScript;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        propulsionScript = this.GetComponent<PlayerPropulsion>();
    }

    void OnCollisionEnter(Collision collision){
        bool isGravitationalObj = collision.gameObject.GetComponent<BaseGravitation>();
        bool hasLowerMass = (collision.rigidbody ? true : false) && (collision.rigidbody.mass < rb.mass);

        if (isGravitationalObj && hasLowerMass){
            float colliderMass = collision.rigidbody.mass;
            if (collision.gameObject.tag == "Player"){
                print("hit player");
            }

            Destroy(collision.gameObject);
            propulsionScript.ChangeMass((int) colliderMass);
        }
    }
}

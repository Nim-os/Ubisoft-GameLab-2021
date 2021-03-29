using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassPickup : MonoBehaviour
{
    public float amount;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            PlayerPropulsion propulsionScript = other.gameObject.GetComponent<PlayerPropulsion>();
            propulsionScript.ChangeMass(amount);

            Destroy(gameObject);
        }
    }
}

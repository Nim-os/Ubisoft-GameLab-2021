using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Player Propulsion class
    Contains all methods related to self propulsion
*/
public class PlayerPropulsion : MonoBehaviour
{
    public float propulsionForce;
    public int gas;

    private Rigidbody rb;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // On mouse button held down && not empty on gas,
        // add force to player towards the mouse direction
        if (Input.GetMouseButton(0) && gas > 0)
        {
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

        // Reset location for testing purposes
        if (Input.GetKeyDown("r"))
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.transform.position = Vector3.zero;
        }
    }
}

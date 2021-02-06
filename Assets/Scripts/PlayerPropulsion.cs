using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropulsion : MonoBehaviour
{
    public float propulsionForce;
    public int gas;

    private Rigidbody rigidbody;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // hold down mouse button
        if (Input.GetMouseButton(0) && gas > 0)
        {
            gas--;
            // use mouse location to calculate direction to apply force
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;
            if (plane.Raycast(ray, out enter))
            {
                var hitPoint = ray.GetPoint(enter);
                var mouseDirection = hitPoint - gameObject.transform.position;
                mouseDirection = mouseDirection.normalized;
                rigidbody.AddForce(mouseDirection * propulsionForce, ForceMode.Impulse);
            }
        }

        // reset location for testing purposes
        if (Input.GetKeyDown("r"))
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.transform.position = Vector3.zero;
        }
    }
}

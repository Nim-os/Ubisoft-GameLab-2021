using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    /// <summary> Returns Vector3 position of mouse on plane</summary>
    public static Vector3 mousePositionOnPlane(){
        // change to Physics.Raycast to ignore layer?
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter;

        if (plane.Raycast(ray, out enter)){
            var hitPoint = ray.GetPoint(enter);
            return hitPoint;
        }
        return Vector3.zero;
    }

    /// <summary> Gets mouse direction in relation to a gameobject </summary>
    public static Vector3 GetMouseDirection(GameObject obj){
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter;

        if (plane.Raycast(ray, out enter))
        {
            var hitPoint = ray.GetPoint(enter);
            var mouseDirection = hitPoint - obj.transform.position;
            mouseDirection = mouseDirection.normalized;
            return mouseDirection;
        }
        // did not hit
        return Vector3.zero;
    }
}
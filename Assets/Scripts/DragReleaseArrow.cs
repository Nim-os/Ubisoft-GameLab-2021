using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragReleaseArrow : MonoBehaviour
{
    public GameObject arrowPrefab;
    GameObject currentArrow;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (currentArrow == null)
            {
                currentArrow = Instantiate(arrowPrefab, this.transform.position, Quaternion.identity, this.transform);
            }
            currentArrow.transform.localScale = new Vector3(Utils.DistanceMouseObj(this.gameObject) * .1f, 1, 1);

            Vector3 mousePos = Input.mousePosition;
            Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - playerPos.x;
            mousePos.y = mousePos.y - playerPos.y;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            currentArrow.transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Destroy(currentArrow);
        }
    }
}

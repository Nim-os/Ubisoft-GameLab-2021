using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragReleaseArrow : MonoBehaviour
{
    public InputSystem input;

    public GameObject arrowPrefab;
    GameObject currentArrow;

    Vector2 mousePos = Vector2.zero;
    bool holding;

    void Awake()
    {
        input = new InputSystem();

        input.Game.Primary.performed += x => holding = true;
        input.Game.Primary.canceled += x => holding = false;

        input.Game.MousePosition.performed += x => mousePos = x.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (holding)
        {
            if (currentArrow == null)
            {
                if (gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
                {
                    currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform);
                }
            }
            if (gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
            {
                currentArrow.transform.localScale = new Vector3(.8f, .5f, 1); // Length of arrow is fixed relative to player size.

                Vector2 mousePosTemp = Vector2.zero;
                Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
                mousePosTemp.x = mousePos.x - playerPos.x;
                mousePosTemp.y = mousePos.y - playerPos.y;
                float angle = Mathf.Atan2(mousePosTemp.y, mousePosTemp.x) * Mathf.Rad2Deg;
                currentArrow.transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
            }
        }
        if (!holding && currentArrow != null)
        {
            if (currentArrow != null) Destroy(currentArrow);
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

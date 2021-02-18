using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingTimeOut : MonoBehaviour
{
    Image img;
    Color clr;

    void Awake()
    {
        gameObject.transform.SetParent(Ping.instance.GetCanvas().transform);

        transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    void Start()
    {
        img = GetComponent<Image>();
        clr = img.color;
    }

    void Update()
    {
        img.color = new Color(clr.r, clr.g, clr.b, img.color.a - (0.5f*Time.fixedDeltaTime));
        if (GetComponent<Image>().color.a <= 0) Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingLoad : MonoBehaviour
{
    void Awake()
    {
        gameObject.transform.SetParent(Ping.instance.GetCanvas().transform);

        transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

}

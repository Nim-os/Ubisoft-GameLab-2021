using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.transform.SetParent(Ping.instance.GetCanvas().transform);
    }

}

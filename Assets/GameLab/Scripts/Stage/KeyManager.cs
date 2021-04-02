using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    private List<GameObject> keys = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform key in this.transform)
        {
            keys.Add(key.gameObject);
        }
    }

    public int GetKeysCount()
    {
        return keys.Count;
    }

}

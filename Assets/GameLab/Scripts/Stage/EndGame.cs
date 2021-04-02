using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour
{
    public KeyManager keyManager;
    private int ct;

    void Start()
    {
        this.gameObject.SetActive(false);
        keyManager = GameObject.FindGameObjectWithTag("Key-m").GetComponent<KeyManager>();
    }

    void Update()
    {
        ct = keyManager.GetKeysCount();

        if (ct == 0) this.gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") SceneManager.LoadScene(4);

    }


}

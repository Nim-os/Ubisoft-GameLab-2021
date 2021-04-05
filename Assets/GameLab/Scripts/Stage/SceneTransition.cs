using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

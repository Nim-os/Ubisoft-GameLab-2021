using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int currentKeys = 0;
    [SerializeField] GameObject AsteroidSpawners;
    [SerializeField] GameObject BlackHoles;
    
    // Update is called once per frame
    void Update()
    {
        if (currentKeys == 2) {
            ActivateAsteroidSpawners();
        }else if (currentKeys == 4) {
            ActivateBlackHoles();
        }
    }

    void ActivateAsteroidSpawners(){
        AsteroidSpawners.SetActive(true);
    }

    void ActivateBlackHoles(){
        BlackHoles.SetActive(true);
    }

    public void NewKey() {
        currentKeys++;
    }
}

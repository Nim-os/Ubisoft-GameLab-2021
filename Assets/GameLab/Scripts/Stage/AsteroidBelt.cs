using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBelt : MonoBehaviour
{
    public const float maxWait = 5f;
    private float wait;
    
    // [SerializeField]
    // private GameObject asteroid;

    void Start()
    {
        // Want only the host to handle asteroids, destroy if not host
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
		{
            Destroy(this);
            return;
		}

        wait = Random.Range(1, maxWait);
    }

    void Update()
    {
        if (wait <= 0)
        {
            wait = maxWait;

            SpawnAsteroid();
        }

        wait -= Time.deltaTime;
    }

    void SpawnAsteroid()
    {
        Photon.Pun.PhotonNetwork.Instantiate("BasicAsteroid", transform.position, Quaternion.identity);
    }
}

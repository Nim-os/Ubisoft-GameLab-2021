using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{
    public GameObject asteroid;
    public float spawnTimerMax = 10;
    float spawnTimer;

    void Start()
    {
        spawnTimer = spawnTimerMax;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnTimerMax;

            GameObject ast = Instantiate(asteroid, transform.position, Quaternion.identity);
            ast.GetComponent<BaseAsteroid>().xthurst = 10;
            ast.GetComponent<BaseAsteroid>().zthrust = -15;

            Destroy(ast, 30); // Destroy asteroids after 30 seconds.
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBelt : MonoBehaviour
{
    public float maxWait = 5f;
    float wait;
    
    [SerializeField]
    private GameObject asteroid;

    void Start()
    {
        wait = Random.Range(1, maxWait);
    }

    void Update()
    {
        if (wait <= 0)
        {
            SpawnAsteroid();
            wait = maxWait;
        }
        wait -= Time.deltaTime;
    }

    void SpawnAsteroid()
    {
        GameObject ast = Instantiate(asteroid, transform.position, Quaternion.identity);
        ast.GetComponent<BaseAsteroid>().xthurst = 10;
        ast.GetComponent<BaseAsteroid>().zthrust = -15;
        Destroy(ast, 5);
    }
}

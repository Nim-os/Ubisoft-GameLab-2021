using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{
    public float waitTime = 10f;
    
    [SerializeField]
    private GameObject asteroid;

    void Start()
    {
        StartCoroutine(SpawnAsteroid(waitTime));
    }

    IEnumerator SpawnAsteroid(float wait)
    {
        GameObject ast = Instantiate(asteroid, transform.position, Quaternion.identity);
        ast.GetComponent<BaseAsteroid>().xthurst = 10;
        ast.GetComponent<BaseAsteroid>().zthrust = -15;
        Destroy(ast, 30);

        yield return new WaitForSeconds(wait);
    }
}

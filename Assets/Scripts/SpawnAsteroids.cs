using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{
    public GameObject asteroid;
    public Mesh a1;
    public Material a1_mat;
    public Mesh a2;
    public Material a2_mat;
    public Mesh a3;
    public Material a3_mat;
    public Mesh a4;
    public Material a4_mat;
    public Mesh a5;
    public Material a5_mat;

    Mesh[] meshes = new Mesh[5];
    Material[] materials = new Material[5];

    public float spawnTimerMax = 10;
    float spawnTimer;

    void Start()
    {
        spawnTimer = spawnTimerMax;

        meshes[0] = Instantiate(a1);
        meshes[1] = Instantiate(a2);
        meshes[2] = Instantiate(a3);
        meshes[3] = Instantiate(a4);
        meshes[4] = Instantiate(a5);
        materials[0] = a1_mat;
        materials[1] = a2_mat;
        materials[2] = a3_mat;
        materials[3] = a4_mat;
        materials[4] = a5_mat;

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

            int randomizer = Random.Range(0, 5); // Randomly skins the asteroid spawned.
            ast.GetComponent<MeshFilter>().mesh = meshes[randomizer];
            ast.GetComponent<MeshRenderer>().material = materials[randomizer];

            Destroy(ast, 30); // Destroy asteroids after 30 seconds.
        }
    }
}

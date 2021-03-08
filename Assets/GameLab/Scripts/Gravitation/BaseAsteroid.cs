using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAsteroid : MonoBehaviour
{
    public float xthurst;
    public float zthrust;

    private Rigidbody rb;

    [SerializeField]
    private List<Mesh> models;
    [SerializeField]
    private List<Material> textures;

    void Start(){
        // Set private variables
        rb = gameObject.GetComponent<Rigidbody>();
        int choice = Random.Range(0,models.Count);
        SetModelTexture(models[choice], textures[choice]);
    }

    private void Update()
    {
        rb.AddForce(new Vector3 (xthurst, 1, zthrust));
    }

    private void SetModelTexture(Mesh obj, Material mat){
        GetComponent<MeshFilter>().mesh = obj;
        GetComponent<MeshRenderer>().material = mat;
    }
}

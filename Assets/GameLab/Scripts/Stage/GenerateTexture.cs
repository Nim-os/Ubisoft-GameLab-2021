using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTexture : MonoBehaviour
{
    [SerializeField]
    private List<Mesh> models;
    [SerializeField]
    private List<Material> textures;

    // Start is called before the first frame update
    void Start()
    {
        int choice = Random.Range(0,models.Count);
        SetModelTexture(models[choice], textures[choice]);
    }

    private void SetModelTexture(Mesh obj, Material mat){
        GetComponent<MeshFilter>().mesh = obj;
        GetComponent<MeshRenderer>().material = mat;
    }
}

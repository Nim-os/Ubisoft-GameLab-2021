using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NetworkPrefab
{
    public GameObject Prefab;
    public string Path;

    public NetworkPrefab(GameObject obj, string path)
    {
        Prefab = obj;
        Path = GetPrefabPath(path);
    }

    private string GetPrefabPath(string path)
    {
        int extension = System.IO.Path.GetExtension(path).Length;
        int startIndex = path.ToLower().IndexOf("resources");
        // If resources not found for some reason
        if (startIndex == -1)
        {
            Debug.LogError("WARNING: The path specificed does not contain a \"Resources\" directory.");
            return string.Empty;
        } else 
        {
            // Looks for 'Resources' directory within specified path.
            return path.Substring(startIndex+10, path.Length - (startIndex + extension));
        }
    }

}

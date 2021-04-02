using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Photon.Pun;
using Photon.Realtime;
[CreateAssetMenu(menuName = "Singletons/LevelManager")]

public class LevelManager : SingletonScriptableObject<LevelManager>
{
    [SerializeField]
    private GameSettings _gameSettings;
    private List<NetworkPrefab> _networkPrefabs = new List<NetworkPrefab>();
    public static GameSettings GameSettings 
    {
        get {return Instance._gameSettings;}
    }

    public static GameObject NetworkInstantiate(GameObject obj, Vector3 pos, Quaternion rot)
    {
        
        foreach (NetworkPrefab item in Instance._networkPrefabs)
        {
            if (item.Prefab == obj)
            {
                if (item.Path != string.Empty)
                {
                    GameObject x = PhotonNetwork.Instantiate(item.Path, pos, rot);
                    return x;
                }
                else Debug.LogError("No GameObject \""+item.Prefab+"\" found on path: \n"+item.Path);
            }
        }

        return null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void PopulateNetworkedPrefabs()
    {
#if UNITY_EDITOR
        Instance._networkPrefabs.Clear();
        GameObject[] res = Resources.LoadAll<GameObject>("");
        for (int i=0; i< res.Length; i++)
        {
            if (res[i].GetComponent<PhotonView>() != null)
            {
                string path = AssetDatabase.GetAssetPath(res[i]);
                Instance._networkPrefabs.Add(new NetworkPrefab(res[i], path));
            }
        }
#endif
    }
}

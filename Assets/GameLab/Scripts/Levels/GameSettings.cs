using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "0.0.0";
    [SerializeField]
    private string _nickName = "April";
    public string GameVersion
    {
        get {return _gameVersion;}
    }
    public string NickName
    {
        
        get 
        {
            int r = Random.Range(0,999999);
            return _nickName + r.ToString();
        }
    }

}

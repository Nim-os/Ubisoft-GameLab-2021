using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singletons/LevelManager")]

public class LevelManager : SingletonScriptableObject<LevelManager>
{
    [SerializeField]
    private GameSettings _gameSettings;
    public static GameSettings GameSettings 
    {
        get {return Instance._gameSettings;}
    }
}

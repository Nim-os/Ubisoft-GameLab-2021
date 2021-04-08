using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;

    void OnEnable(){
        Time.timeScale=0f;
        PauseMenu.GameIsPaused=true;
    }

    public void BacktoLobby(){
        
        ServerManager.instance.KickAll();
    }


    public void RestartLevel(){
        Time.timeScale=1f;
        PauseMenu.GameIsPaused=false;

        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        canvas.SetActive(false);
    }
}

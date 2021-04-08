using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;
    public GameObject gasbarUI;
    public GameObject gastextUI;
    public GameObject gasBoarder;

    void OnEnable(){
        Time.timeScale=0f;
        PauseMenu.GameIsPaused=true;

        gasbarUI.SetActive(false);
        gastextUI.SetActive(false);
        gasBoarder.SetActive(false);
    }

    public void BacktoLobby(){
        Time.timeScale=1f;
        PauseMenu.GameIsPaused=false;

        gasbarUI.SetActive(true);
        gastextUI.SetActive(true);
        gasBoarder.SetActive(true);
        
        ServerManager.instance.KickAll();
    }


    public void RestartLevel(){
        Time.timeScale=1f;
        PauseMenu.GameIsPaused=false;

        gasbarUI.SetActive(true);
        gastextUI.SetActive(true);
        gasBoarder.SetActive(true);

        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        canvas.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;
    public GameObject inGameCanvas;
    public GameObject gasbarUI;


    void OnEnable(){
        Time.timeScale=0f;
        PauseMenu.GameIsPaused=true;

        gasbarUI = GameObject.Find("GasBar");


        gasbarUI.SetActive(false);
        inGameCanvas.SetActive(false);

    }

    public void BacktoLobby(){
        Time.timeScale=1f;
        PauseMenu.GameIsPaused=false;

        gasbarUI.SetActive(true);
        inGameCanvas.SetActive(true);


        ServerManager.instance.KickAll();
    }


    public void RestartLevel(){
        gasbarUI.SetActive(true);
        inGameCanvas.SetActive(true);

        Time.timeScale=1f;
        PauseMenu.GameIsPaused=false;


        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        canvas.SetActive(false);
    }
}

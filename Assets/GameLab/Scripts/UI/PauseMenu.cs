using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public static bool GameIsPaused = false;
    public static bool disconnecting=false;

    public InputSystem input;

    public GameObject PauseMenuUI;
    public GameObject gasbarUI;
    public GameObject gastextUI;
    public GameObject gasBoarder;

	private void Awake()
	{
        input = new InputSystem();

        input.Game.Pause.performed += x =>
        {
            if (!disconnecting)
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        };
	}



    public void Resume(){
        PauseMenuUI.SetActive(false);
        gasbarUI.SetActive(true);
        gastextUI.SetActive(true);
        gasBoarder.SetActive(true);
        Time.timeScale=1f;
        GameIsPaused=false;
    }


    void Pause(){
        PauseMenuUI.SetActive(true);
        gasbarUI.SetActive(false);
        gastextUI.SetActive(false);
        gasBoarder.SetActive(false);
        Time.timeScale=0f;
        GameIsPaused=true;
    }


    public void backToLobby(){
        disconnecting=true;

        Time.timeScale = 1f;

        ServerManager.instance.Close();
        SceneManager.LoadScene(0);
    }

	private void OnEnable()
	{
        input.Enable();
	}

	private void OnDisable()
	{
        input.Disable();
	}

}

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
    [SerializeField]
    private GameObject gasbarUI;

	private void Awake()
	{
        gasbarUI = GameObject.Find("GasBar");
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
        Time.timeScale=1f;
        GameIsPaused=false;
    }


    void Pause(){
        PauseMenuUI.SetActive(true);
        gasbarUI.SetActive(false);
        Time.timeScale=0f;
        GameIsPaused=true;
    }


    public void backToLobby()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;

        ServerManager.instance.KickAll();
    }

    public void Restart()
	{
        Time.timeScale = 1f;
        GameIsPaused = false;

        ServerManager.instance.RestartLevel();
	}

	private void OnEnable()
	{
        input.Enable();
	}

	private void OnDisable()
	{
        input.Disable();
        Time.timeScale = 1f;
    }

}

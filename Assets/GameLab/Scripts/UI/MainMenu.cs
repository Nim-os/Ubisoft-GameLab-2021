﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioMixer mix;

    public void PlayGame()
    {
        // Currently set to 1: Dev Lobby
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume (float vol)
    {
        // Set mix float.
        mix.SetFloat("Volume", vol);
        // Save Mix float in PlayerPrefs
        PlayerPrefs.SetFloat("Volume", vol);
        PlayerPrefs.Save();
    }
}

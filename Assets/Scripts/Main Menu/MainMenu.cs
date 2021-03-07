using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioMixer mix;
    public void PlayGame()
    {
        // Currently set to next scene in Build queue, but can be changed to the lobby or tutorial level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        // Currently set to next scene in Build queue, but can be changed to the lobby or tutorial level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetVolume (float vol)
    {
        mix.SetFloat("volume", vol);
    }


}

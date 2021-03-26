using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSync : MonoBehaviour
{
    // get Audio slider
    public Slider audioSlider;
    public AudioMixer mix;
    void Awake ()
    {
        // Use PlayerPrefs to get Key containing volume float.
        if (audioSlider != null && PlayerPrefs.HasKey("Volume")) 
        {
            float currentVolume = PlayerPrefs.GetFloat("Volume");
            audioSlider.value = currentVolume;
            mix.SetFloat("Volume", currentVolume);
        }
    }

    void Update ()
    {
        mix.SetFloat("Volume", audioSlider.value);
        // Tee-hee
        PlayerPrefs.SetFloat("Volume", audioSlider.value);
        PlayerPrefs.Save();
        
    }
}

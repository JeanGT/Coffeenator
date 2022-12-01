using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public AudioMixer mixerMusica;
    public AudioMixer mixerFX;

    public Slider volumeMusica;
    public Slider volumeSFX;

    void Start()
    {
        float value;
        bool result = mixerMusica.GetFloat("volumeMusica", out value);
        if (result)
        {
            volumeMusica.value = value;
        }

        value = 0;
        result = mixerFX.GetFloat("volumeFX", out value);
        if (result)
        {
            volumeSFX.value = value;
        }
    }

    void Update()
    {
    
    }
}

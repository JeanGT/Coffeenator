using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OpcoesScript : MonoBehaviour
{
    public AudioMixer mixerMusica;
    public AudioMixer mixerFX;

    public void mudarVolumeMusica(float volume)
    {
        if (volume > -31f)
        {
            mixerMusica.SetFloat("volumeMusica", volume);
        }
        else
        {
            mixerMusica.SetFloat("volumeMusica", -80f);
        }
    }

    public void mudarVolumeFX(float volume)
    {
        if (volume > -31f) {
            mixerFX.SetFloat("volumeFX", volume);
        }
        else
        {
            mixerFX.SetFloat("volumeFX", -80f);
        }
    }
}

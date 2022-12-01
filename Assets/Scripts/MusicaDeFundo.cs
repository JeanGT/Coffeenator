using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaDeFundo : MonoBehaviour
{
    public AudioSource[] a;
    public AudioSource background;
    public AudioClip[] efeitos;
    public bool doNotDestroy;

    private static bool foiCriado;

    void Start()
    {
        if (!foiCriado) {
            foiCriado = doNotDestroy;
            if (doNotDestroy) {
                DontDestroyOnLoad(this.gameObject);
            }
        } else {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setBackground(AudioClip bg) {
        background.clip = bg;
    }

    public void pauseBg() {
        background.Pause();
    }

    public void playBg() {
        background.Play();
    }
    public void unPauseBg()
    {
        background.UnPause();
    }

    public void playSound(int sound) {
        playSound(efeitos[sound]);
    }

    public void playSound(AudioClip sound) {
        for(int i = 0; i < a.Length; i++) {
            if (!a[i].isPlaying)
            {
                a[i].clip = sound;
                a[i].Play();
                break;
            }
        }
    }
}

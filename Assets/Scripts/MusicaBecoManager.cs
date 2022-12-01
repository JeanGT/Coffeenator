using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaBecoManager : MonoBehaviour
{
    public AudioClip musicaTensa;
    public float aliadoOffset;

    void Start()
    {
        MusicaDeFundo caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();

        if (caixaDeSom != null)
        {
            if (PlayerStatus.getProgresso() > 7 && PlayerStatus.getProgresso() < 18 && !(PlayerStatus.isChaveMendigo() || PlayerStatus.getAvisoMendigo() == -1))
                caixaDeSom.setBackground(musicaTensa);
                caixaDeSom.playBg();
        }
    }

    void Update()
    {

    }
}

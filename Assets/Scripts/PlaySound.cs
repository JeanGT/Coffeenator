using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaySound : MonoBehaviour
{
    private MusicaDeFundo caixaDeSom; 
    void Start()
    {
        try
        {
            caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
        }
        catch (NullReferenceException)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound(int sound)
    {
        if(caixaDeSom != null)
        caixaDeSom.playSound(sound);
    }
}

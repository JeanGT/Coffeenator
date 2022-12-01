using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDerrota : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
        if (caixaDeSom != null)
        {
            caixaDeSom.GetComponent<MusicaDeFundo>().pauseBg();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

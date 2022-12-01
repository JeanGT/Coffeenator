using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Janela : MonoBehaviour
{
    UnityEngine.Rendering.Universal.Light2D sunLight;
    UnityEngine.Rendering.Universal.Light2D myLight;

    void Start()
    {
        sunLight = GameObject.Find("Sun").GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        myLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myLight.color = sunLight.color;
    }
}

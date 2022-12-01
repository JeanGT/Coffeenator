using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DiminuirIntensidade : MonoBehaviour {
    UnityEngine.Rendering.Universal.Light2D luz;
    public float velocidade;

    public bool comecou;

    void Start() {
        luz = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    void Update() {
        if (comecou) {
            if (luz.intensity > 0) {
                luz.intensity -= velocidade * Time.deltaTime;
            } else {
                luz.intensity = 0;
            }
        } 
    }
}

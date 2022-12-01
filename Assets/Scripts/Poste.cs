using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Poste : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D luz;
    void Start()
    {
        luz = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        luz.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStatus.horario > ((Sol.duracaoTardinha - Sol.duracaoDia) / 2) + Sol.duracaoDia) {
            luz.enabled = true;
        } else {
            luz.enabled = false;
        }
    }
}

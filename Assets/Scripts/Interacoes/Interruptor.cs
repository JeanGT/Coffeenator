using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Interruptor : Interacao
{
    public bool on;

    public UnityEngine.Rendering.Universal.Light2D[] luzes;
    public string tituloApagar;
    public bool quarto;

    void Start() {
        if ((quarto && PlayerStatus.isInterruptorQuarto()) || (!quarto && PlayerStatus.isInterruptorCasa()))
        {
            setOn(!on);

            if (quarto)
            {
                PlayerStatus.setInterruptorQuarto(on);
            }
            else
            {
                PlayerStatus.setInterruptorCasa(on);
            }
        }
    }

    public override void interagir() {
        setOn(!on);
        string aux = tituloInteracao;
        tituloInteracao = tituloApagar;
        tituloApagar = aux;

        if (on)
        {
            GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(62);
        }
        else
        {
            GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(63);
        }

        if (quarto) {
            PlayerStatus.setInterruptorQuarto(on);
        } else {
            PlayerStatus.setInterruptorCasa(on);
        }
    }

    private void setOn(bool on) {
        this.on = on;
        attLights();
    }

    public void attLights() {
        for(int i = 0; i < luzes.Length; i++) {
            luzes[i].enabled = on;
        }
    }

}

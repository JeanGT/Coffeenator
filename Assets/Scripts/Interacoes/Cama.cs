using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cama : Interacao
{
    public Interruptor interruptor;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void interagir() {
        if (PlayerStatus.getProgresso() > 7) {
            GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(8);
            if (PlayerStatus.horario > Sol.horarioDormir) {
                if (interruptor.on) {
                    interruptor.interagir();
                }
                NPCSpawn.npcsDia = false;
                PlayerStatus.horario = 0;
                GameManager.showMessage("Você teve uma boa noite de sono. Agora está de dia.");
            } else {
                if (interruptor.on)
                {
                    interruptor.interagir();
                }
                NPCSpawn.npcsDia = true;
                PlayerStatus.horario = Sol.duracaoTardinha;
                GameManager.showMessage("Você tirou um cochilho. Agora está de noite.");
            }
        } else
        {
            GameManager.showMessage("Você não pode dormir no momento.");
        }
    }
}

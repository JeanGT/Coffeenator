using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteracaoBoxComp : MonoBehaviour { 
    private static Interacao objeto;

    public void onClick() {
        objeto.interagir();
    }

    public void show(Interacao objeto) {
        InteracaoBoxComp.objeto = objeto;
        GetComponent<Text>().text = objeto.getTituloInteracao();
    }
}

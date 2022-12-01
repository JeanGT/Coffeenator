using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AcharBilhete : MonoBehaviour {
    public SpriteRenderer spriteJanelaQuebrada;
    public SpriteRenderer spriteJanelaNormal;
    public int progressoParaLer = 6;

    void Start() {
        if (PlayerStatus.getProgresso() >= progressoParaLer) {
            if(spriteJanelaQuebrada != null)
            spriteJanelaQuebrada.enabled = true;

            if(spriteJanelaNormal != null)
            spriteJanelaNormal.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversaPaga : Conversa
{
    public float preco;
    public string[] recompensas;
    public Text txtPreco;
    public Text txtRecompensas;
    public GameObject canvas;
    public string[] conversaSemDinheiro;
    public Sprite[] spritesSemDinheiro;

    private bool pagou;

    protected override void Start() {
        base.Start();
        canvas.SetActive(false);
    }

    public override void interagir() {
        if (PlayerStatus.getDinheiros() >= preco) {
            PlayerStatus.setDinheiros(PlayerStatus.getDinheiros() - preco);
            pagou = true;
        } else {
            conversa.falas = conversaSemDinheiro;
            conversa.sprites = spritesSemDinheiro;
        }
        base.interagir();
    }

    protected override void hideDialogueBox() {
        if (pagou) {
            base.hideDialogueBox();
        } else {
            conversationBox.SetActive(false);
        }
    }

    public void mostrarMsgPreco() {
        canvas.SetActive(true);
        txtPreco.text = "R$" + preco.ToString("0.00");
        txtRecompensas.text = "";
        for (int i = 0; i < recompensas.Length; i++) {
            txtRecompensas.text += "+ " + recompensas[i] + "\n";
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        canvas.SetActive(false);
    }
}

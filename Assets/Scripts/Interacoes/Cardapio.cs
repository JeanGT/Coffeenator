using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardapio : Interacao {
    GameObject cardapio;

    public int progressoMinimo;

    private const float precoCafe = 1;
    private const float precoAgua = 1;
    private const float precoComida = 1;
    private BoxCollider2D myCollider;


    private const string msgSemDinheiro = "Você não tem dinheiro suficiente";

    // Use this for initialization
    void Start() {
        StartObjeto();
        cardapio = GameObject.Find("CardapioP");
        cardapio.SetActive(false);
        myCollider = GetComponent<BoxCollider2D>();
        if(PlayerStatus.getProgresso() < progressoMinimo) {
            myCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        UpdateObjeto();
    }

    public override void interagir() {
        cardapio.SetActive(true);
        GameObject.Find("Player").GetComponent<Player>().setFreeze(true);
    }

    private bool efetuarCompra(float valor, string msg) {
        if(PlayerStatus.getDinheiros() >= valor) {
            PlayerStatus.setDinheiros(PlayerStatus.getDinheiros() - valor);
            GameManager.showMessage(msg);
            return true;
        }
        GameManager.showMessage(msgSemDinheiro);
        return false;
    }

    public void comprarCafe() {
        efetuarCompra(precoCafe, "Você comprou aquele cafezinho!");
        cardapio.SetActive(false);
    }

    public void comprarAgua() {
        efetuarCompra(precoAgua, "Você comprou água.");
        cardapio.SetActive(false);
    }

    public void comprarComida() {
        efetuarCompra(precoComida, "Você comprou comida.");
        cardapio.SetActive(false);
    }

    public void fecharCardapio() {
        GameObject.Find("Player").GetComponent<Player>().setFreeze(false);
        cardapio.SetActive(false);
    }

}

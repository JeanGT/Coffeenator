using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float velocidade;
    public float minX;
    public float maxX;
    public AudioSource passos;

    private Transform msgInteracao;
    private bool isFreezed = false;
    private Animator animator;
    private float initialXScale;
    private BoxCollider2D colliderPlayer;
    private Button btnInteracao;

    // Use this for initialization
    void Awake () {
        colliderPlayer = GetComponent<BoxCollider2D>();
        msgInteracao = GameObject.Find("txtInteracao").transform;
        msgInteracao.gameObject.SetActive(false);
        animator = transform.Find("sprites").GetComponent<Animator>();
        btnInteracao = GameObject.Find("BotaoInteracao").GetComponent<Button>();
        btnInteracao.interactable = false;
        initialXScale = animator.transform.localScale.x;
    }

    private void Start()
    {
        passos.Play();
        passos.Pause();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!isFreezed) {
            colliderPlayer.enabled = true;
            mover();
            corrigirPosicao();
        } else {
            colliderPlayer.enabled = false;
        }
    }

    public void setFreeze(bool freeze) {
        this.isFreezed = freeze;

        animator.SetBool("movendo", false);
        passos.Pause();
    }

    public bool getIsFreezed()
    {
        return isFreezed;
    }

    private void mover()
    {
        float axisHorizontalMove = 0;
        axisHorizontalMove = MyInput.getHorizontalAxis();
        
        transform.position += new Vector3(axisHorizontalMove * Time.fixedDeltaTime * velocidade, 0, 0);

        if (axisHorizontalMove == 0)
        {
            passos.Pause();
        }
        else
        {
            passos.UnPause();
        }

        animator.SetBool("movendo", axisHorizontalMove != 0);
        if(axisHorizontalMove < 0) {
            lookLeft();
        } else if (axisHorizontalMove > 0){
            lookRight();
        }
    }

    public void lookLeft() {
        animator.transform.localScale = new Vector3(-initialXScale, animator.transform.localScale.y, animator.transform.localScale.z);
    }

    public bool isLookingLeft()
    {
        return animator.transform.localScale.x == -initialXScale;
    }

    public void lookRight() {
        animator.transform.localScale = new Vector3(initialXScale, animator.transform.localScale.y, animator.transform.localScale.z);
    }

    private void corrigirPosicao() {
        if(transform.position.x > maxX) {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        } else if (transform.position.x < minX) {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.GetComponent<Interacao>() != null) {
            Bob bob = other.gameObject.GetComponent<Bob>();
            if (bob != null && PlayerStatus.getProgresso() == bob.progressoComprasDesabilitadas) {
            } else if (bob != null && PlayerStatus.getProgresso() == bob.progressoReceberChave) {
                bob.EntregarChave();
            }

            Porta porta = other.gameObject.GetComponent<Porta>();
            if(porta != null && porta.isAutomatico()) {
                porta.interagir();
                return;
            }

            Component[] conversas = other.gameObject.GetComponents(typeof(Conversa));
            int conversasSize = conversas.Length;

            foreach (Conversa conversa in conversas) {
                if (conversa.enabled) {
                    bool temDash =  conversa.getNivelMinimoDash() <= PlayerStatus.nivelDash && conversa.getNivelMaximoDash() >= PlayerStatus.nivelDash;
                    bool temProjetil = conversa.getNivelMinimoProjetil() <= PlayerStatus.nivelProjetil && conversa.getNivelMaximoProjetil() >= PlayerStatus.nivelProjetil;
                    bool temEspecial = conversa.getNivelMinimoEspecial() <= PlayerStatus.nivelEspecial && conversa.getNivelMaximoEspecial() >= PlayerStatus.nivelEspecial;
                    bool avisoMendigo =  (conversa.precisaAvisoMendigo && !(PlayerStatus.getAvisoMendigo() == 1)) || (conversa.precisaAvisoMendigo && PlayerStatus.getProgresso() > 12);
                    bool superUmaVez = conversa.jaFoiExecutada() && conversa.superUmaVez;

                    if (PlayerStatus.getProgresso() < conversa.getProgressoMinimo() || PlayerStatus.getProgresso() > conversa.getProgressoMaximo() || !temDash || !temEspecial || !temProjetil || avisoMendigo || superUmaVez) {
                        continue;
                    }
                    if (conversa.getAutomatico()) {
                        //if (!conversa.wasUsed()) {
                            conversa.interagir();
                        //}
                        continue;
                    } else {
                        btnInteracao.interactable = true;
                        msgInteracao.gameObject.SetActive(true);
                        ConversaPaga conversaPaga = null;
                        if(conversa.GetType() == typeof(ConversaPaga)) {
                            conversaPaga = ((ConversaPaga)conversa);
                        }
                        if(conversaPaga != null) {
                            conversaPaga.mostrarMsgPreco();
                        }
                        msgInteracao.GetComponent<InteracaoBoxComp>().show(conversa);
                        break;
                    }
                } else {
                    conversasSize--;
                }
            }

            if (bob != null && PlayerStatus.getProgresso() == bob.progressoComprasDesabilitadas) {
                return;
            }

            bool ehConversa = other.GetComponent<Interacao>() is Conversa;

            if (!ehConversa) {
                if (other.GetComponent<Interacao>().getTituloInteracao() != "") {
                    btnInteracao.interactable = true;
                    msgInteracao.gameObject.SetActive(true);
                    msgInteracao.GetComponent<InteracaoBoxComp>().show(other.GetComponent<Interacao>());
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        msgInteracao.GetComponent<Text>().text = "";
        msgInteracao.gameObject.SetActive(false);
        btnInteracao.interactable = false;
    }
}

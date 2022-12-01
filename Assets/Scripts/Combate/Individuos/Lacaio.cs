using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lacaio : Inimigo
{
    public GameObject projetil;
    public GameObject mira;

    public float tempoAteAparecer;
    public float tempoParado;
    public float tempoMirando;

    private float cTempoParado;
    private float cTempoMirando;
    private bool hasAttacked;
    private bool apareceu;
    private GameObject fumacaI2;

    void Start()
    {
        mostrarVida = false;
        cVelocidade = velocidade;
        InimigoStart();
        anim = transform.GetComponentInChildren<Animator>();

        mySprite.SetActive(false);
    }

    void Update()
    {
        InimigoUpdate();
    }

    void FixedUpdate() {
        InimigoFixedUpdate();   
    }

    protected override void die() {
        Destroy(this.gameObject);
    }

    protected override void move() {
        if (!apareceu) {
            jogarFumaca();
            apareceu = true;
        } else if(fumacaI == null || fumacaI.GetComponent<Fumaca>().desapareceu) {
            if(fumacaI != null) {
                fumacaI.GetComponent<Fumaca>().podeDesaparecer = true;
            }
            mySprite.SetActive(true);
            
            cTempoParado += Time.fixedDeltaTime;
            if (cTempoParado > tempoAteAparecer) {
                if (hasAttacked) {
                    anim.SetBool("atacando", false);
                    if (fumacaI2 == null) {
                        fumacaI2 = Instantiate(fumaca, transform.position, Quaternion.identity);
                    } else if (fumacaI2.GetComponent<Fumaca>().desapareceu) {
                        fumacaI2.GetComponent<Fumaca>().podeDesaparecer = true;
                        Destroy(this.gameObject);
                    }
                } else {
                    if (cTempoParado > tempoParado + tempoAteAparecer) {
                        cTempoMirando += Time.fixedDeltaTime;

                        mira.SetActive(true);

                        anim.SetBool("mirando", true);

                        if (transform.localScale.x == initialXScale) {
                            mira.transform.right = player.transform.position - mira.transform.position;
                        } else {
                            mira.transform.right = mira.transform.position - player.transform.position;
                        }

                        if (cTempoMirando > tempoMirando) {
                            anim.SetBool("atacando", true);
                            mira.SetActive(false);
                            attack(mira.transform.right);
                            cTempoMirando = 0;
                        }
                    }
                }
            }
        }
    }

    private void attack(Vector2 dir) {
        GameObject projetilI = Instantiate(projetil, mira.transform.GetChild(1).position, Quaternion.identity);
        projetilI.GetComponent<Projetil>().shooter = transform;
        hasAttacked = true;
        cortar(dir);
    }

    protected override void onDie()
    {
        throw new System.NotImplementedException();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaixaPreta : Inimigo
{
    public float timeWalking;
    public float timeStopped;
    public bool avancaObjetivo;
    public Transform[] projetilSpawns;
    public GameObject projetilLento;
    public GameObject projetilRapido;
    public GameObject mira;

    private float cTimeWalking;
    private float cTimeStopped;
    private Vector2 walkDir;
    private bool ataqueEmArea;

    void Start()
    {
        cVelocidade = velocidade;
        InimigoStart();
        setWalkDir();
    }

    private void FixedUpdate() {
        InimigoFixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        InimigoUpdate();
    }

    protected override void move() {
        if (!isFreezed && !isStunned)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("atacando")) {
                anim.SetBool("atacando", false);
            }

            cTimeStopped += Time.fixedDeltaTime;
            if (cTimeStopped > timeStopped)
            {
                if (!ataqueEmArea)
                {
                    mira.SetActive(true);
                    if (!player.GetComponent<PlayerCombate>().specialAttacking) {
                        if (transform.localScale.x == initialXScale) {
                            mira.transform.right = player.transform.position - mira.transform.position;
                        } else {
                            mira.transform.right = mira.transform.position - player.transform.position;
                        }
                    }
                } 

                anim.SetBool("andando", true);
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + walkDir.normalized * cVelocidade * Time.fixedDeltaTime);
                cTimeWalking += Time.fixedDeltaTime;
                if (cTimeWalking > timeWalking)
                {
                    anim.SetBool("atacando", true);
                    setWalkDir();
                    cTimeWalking = 0;
                    cTimeStopped = 0;
                    attack();
                }
            } else {
                anim.SetBool("andando", false);
            }
        }
    }

    private void attack() {
        if (ataqueEmArea) {
            for (int i = 0; i < projetilSpawns.Length; i++) {
                GameObject projetilI = Instantiate(projetilLento, projetilSpawns[i].position, Quaternion.identity);
                projetilI.GetComponent<Projetil>().shooter = transform;
            }
        } else {
            GameObject projetilI = Instantiate(projetilRapido, mira.transform.GetChild(1).position, Quaternion.identity);
            projetilI.GetComponent<Projetil>().shooter = transform;

            cortar(mira.transform.right);
            mira.SetActive(false);
        }
        ataqueEmArea = !ataqueEmArea;
    }

    private void setWalkDir() {
        walkDir = (player.position - transform.position).normalized;
    }

    protected override void onDie()
    {
        if (avancaObjetivo) {
            PlayerStatus.fowardObjective(5, 1);
        }
        PlayerStatus.nivelProjetil++;
    }
}

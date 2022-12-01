using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicoleCombate : Individuo
{
    public float healMult;
    public float timeWalking;
    public float timeStopped;
    public float distMin;
    public float tempoMinimoAndando;

    private float cTempoMinimoAndando;
    private float initXScale;
    private float cTimeWalking;
    private float cTimeStopped;
    private Vector2 walkDir;
    private PlayerCombate player;
    private bool primeiroAndando;
    private Transform alex;

    void Start() {
        if (!PlayerStatus.isNicoleActive()) {
            Destroy(this.gameObject);
        }
        invuneravel = true;
        initXScale = mySprite.transform.localScale.x;
        cVelocidade = velocidade;
        player = GameObject.Find("Player").GetComponent<PlayerCombate>();
        alex = GameObject.Find("AlexCombate").transform;
        IndividuoStart();
        setWalkDir();
    }

    private void FixedUpdate() {
        IndividuoFixedUpdate();
    }

    // Update is called once per frame
    void Update() {
        if(player == null)
        {
            anim.SetBool("atacando", false);
            anim.SetBool("andando", false);

            Destroy(this);
        }

        IndividuoUpdate();
    }

    protected override void move() {

        if (alex != null)
        {
            if (transform.position.y < player.transform.position.y)
            {
                if (transform.position.y < alex.transform.position.y)
                {
                    transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
                }
                else
                {
                    transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
                }
            }
            else
            {
                if (transform.position.y < alex.transform.position.y)
                {
                    transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = -1;
                }
                else
                {
                    transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = -2;
                }
            }
        } else
        {
            if (transform.position.y < player.transform.position.y)
            {
                transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
            }
            else
            {
                transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = -1;
            }
        }

        if (!isFreezed) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("atacando")) {
                anim.SetBool("atacando", false);
            }

            cTimeStopped += Time.fixedDeltaTime;
            if (cTimeStopped > timeStopped) {
                if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > distMin || cTempoMinimoAndando > 0) {
                    if (!primeiroAndando) {
                        cTempoMinimoAndando = tempoMinimoAndando;
                        primeiroAndando = true;
                    }

                    cTempoMinimoAndando -= Time.fixedDeltaTime;

                    setWalkDir();
                    rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + walkDir.normalized * cVelocidade * Time.fixedDeltaTime);
                    anim.SetBool("andando", true);
                } else {
                    primeiroAndando = false;
                    anim.SetBool("andando", false);
                }             

                cTimeWalking += Time.fixedDeltaTime;
                if (cTimeWalking > timeWalking) {
                    anim.SetBool("atacando", true);
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
        GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
        if (caixaDeSom != null)
        {
            caixaDeSom.GetComponent<MusicaDeFundo>().playSound(52);
        }

        player.heal(player.HP * healMult);
        player.onGetHeal();
    }

    private void setWalkDir() {
        walkDir = (GameObject.Find("Player").transform.position - transform.position).normalized;
    }

    private GameObject findClosestEnemy() {
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++) {
            float distance = Vector3.Distance(transform.position, enemys[i].transform.position);
            if (distance < closestDistance) {
                closestEnemy = enemys[i];
                closestDistance = distance;
            }
        }
        return closestEnemy;
    }

    protected override void die() {
        throw new System.NotImplementedException();
    }
}

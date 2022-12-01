using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alex : Individuo {
    public float timeWalking;
    public float timeStopped;
    public GameObject projetil;
    public GameObject mira;
    public float distMin;
    public float tempoMinimoAndando;

    private float initXScale;
    private float cTimeWalking;
    private float cTimeStopped;
    private Vector2 walkDir;
    private GameObject inimigo;
    private bool primeiroAndando;
    private float cTempoMinimoAndando;
    private Transform player;
    private Transform nicole;

    void Start() {
        if (!PlayerStatus.isAlexActive()) {
            Destroy(this.gameObject);
        }
        invuneravel = true;
        initXScale = mySprite.transform.localScale.x;
        cVelocidade = velocidade;
        inimigo = findClosestEnemy();
        IndividuoStart();
        player = GameObject.Find("Player").transform;
        nicole = GameObject.Find("NicoleCombate").transform;
        setWalkDir();
    }

    private void FixedUpdate() {
        if (CombateManager.tempoAteAcabar < -1)
        {
            IndividuoFixedUpdate();
        }
    }

    // Update is called once per frame
    void Update() {
        if (player == null)
        {
            anim.SetBool("atacando", false);
            anim.SetBool("andando", false);

            Destroy(this);
        }

        if (CombateManager.tempoAteAcabar < -1)
        {
            IndividuoUpdate();
        }
    }

    protected override void move() {
        if (transform.position.y < player.transform.position.y) {
            if (transform.position.y < nicole.transform.position.y) {
                transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
            } else {
                transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
            }
        } else {
            if (transform.position.y < nicole.transform.position.y) {
                transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = -1;
            } else {
                transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = -2;
            }
        }

        if (!isFreezed) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("atacando")) {
                anim.SetBool("atacando", false);
            }

            inimigo = findClosestEnemy();

            if (inimigo.transform.position.x > transform.position.x) {
                mySprite.transform.localScale = new Vector3(initXScale, mySprite.transform.localScale.y, mySprite.transform.localScale.z);
            } else {
                mySprite.transform.localScale = new Vector3(-initXScale, mySprite.transform.localScale.y, mySprite.transform.localScale.z);
            }

            cTimeStopped += Time.fixedDeltaTime;
            if (cTimeStopped > timeStopped) {
                mira.SetActive(true);

                if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > distMin || cTempoMinimoAndando > 0) {
                    if (!primeiroAndando) {
                        cTempoMinimoAndando = tempoMinimoAndando;
                        primeiroAndando = true;
                    }

                    cTempoMinimoAndando -= Time.fixedDeltaTime;


                    rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + walkDir.normalized * cVelocidade * Time.fixedDeltaTime);
                    setWalkDir();
                    anim.SetBool("andando", true);
                } else {
                    primeiroAndando = false;
                    anim.SetBool("andando", false);
                }

                if (transform.localScale.x != initXScale) {
                    mira.transform.right = (inimigo.transform.position - mira.transform.position).normalized;
                } else {
                    mira.transform.right = (mira.transform.position - inimigo.transform.position).normalized;
                }



                cTimeWalking += Time.fixedDeltaTime;
                if (cTimeWalking > timeWalking) {
                    anim.SetBool("atacando", true);
                    cTimeWalking = 0;
                    cTimeStopped = 0;
                    attack(mira.transform.right);
                }
            } else {
                anim.SetBool("andando", false);
            }
        }
    }

    private void attack(Vector2 dir) {
        GameObject projetilI = Instantiate(projetil, mira.transform.GetChild(1).position, Quaternion.identity);
        projetilI.GetComponent<Projetil>().shooter = transform;

        cortar(dir);
        mira.SetActive(false);
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

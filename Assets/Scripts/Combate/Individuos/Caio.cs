using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Caio : Inimigo
{
    public float timeWalking;
    public float timeStopped;
    public float forcaDash;
    public float timeBetweenShots;
    public int nTiros;
    public Transform[] projetilSpawns;
    public Transform[] projetilSpawnsTeleguiado;
    public GameObject projetil1;
    public GameObject projetil2;
    public GameObject projetilTeleguiado;
    public Transform pivot;
    public float precisao;
    public Transform notReverse;

    private int cNTiros;
    private float cTimeBetweenShots;
    private float cTimeWalking;
    private float cTimeStopped;
    private Vector2 walkDir;
    private bool ataque1 = false;
    private bool repetirBolao = false;

    void Start() {
        cVelocidade = velocidade;
        InimigoStart();
        setWalkDir();
    }

    private void FixedUpdate() {
        InimigoFixedUpdate();
    }

    // Update is called once per frame
    void Update() {
        InimigoUpdate();
    }

    protected override void move() {
        if (!isFreezed && !isStunned) {
            if (cTimeWalking < timeWalking) {
                anim.SetBool("andando", true);
                anim.SetBool("carregandoDash", false);
                anim.SetBool("dash", false);
               
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + walkDir.normalized * cVelocidade * Time.fixedDeltaTime);
                cTimeWalking += Time.fixedDeltaTime;
            } else {
                if (cTimeStopped > timeStopped) {
                    attack();
                } else {
                    cTimeStopped += Time.fixedDeltaTime;
                   
                    anim.SetBool("atacando1", false);
                    anim.SetBool("atacando2", false);
                    anim.SetBool("andando", false);
                }
            }
        }
    }

    private void attack() {
        if (ataque1) {
            if (cTimeBetweenShots > timeBetweenShots) {
                cTimeBetweenShots = 0;
                if (cNTiros < nTiros) {
                    shoot1();
                    cNTiros++;
                    if(cNTiros == nTiros) {
                        anim.SetBool("carregandoDash", true);
                    }
                } else {
                    cNTiros = 0;
                    anim.SetBool("dash", true);
                    dash();
                    terminarAtaque();
                }
            } else {
                anim.SetBool("atacando2", true);
                cTimeBetweenShots += Time.fixedDeltaTime;
            }
        } else {
            if (cTimeBetweenShots > timeBetweenShots * 2.5f) {
                cTimeBetweenShots = 0;
                anim.SetBool("atacando1", true);
                shoot2();
                if (repetirBolao) {
                    terminarAtaque();
                } else
                {
                    repetirBolao = true;
                }
            } else {
                cTimeBetweenShots += Time.fixedDeltaTime;
            }
        }
    }

    private void terminarAtaque() {
        ataque1 = !ataque1;
        cTimeStopped = 0;
        repetirBolao = false;
        cTimeWalking = 0;
        
        setWalkDir();
    }

    private void shoot1() {
        pivot.transform.right = player.transform.position - pivot.transform.position;
        pivot.Rotate(new Vector3(0, 0, Random.Range(-precisao, precisao)));

        for (int i = 0; i < projetilSpawns.Length; i++) {
            GameObject projetilI = Instantiate(projetil1, projetilSpawns[i].position, Quaternion.identity);
            projetilI.GetComponent<Projetil>().shooter = transform;
        }

        for (int i = 0; i < projetilSpawnsTeleguiado.Length; i++) {
            GameObject projetilI = Instantiate(projetilTeleguiado, projetilSpawnsTeleguiado[i].position, Quaternion.identity);
            projetilI.GetComponent<Projetil>().shooter = transform;
        }

    }

    private void shoot2() {
        pivot.transform.right = player.transform.position - pivot.transform.position;

        GameObject projetilI = Instantiate(projetil2, projetilSpawns[1].position, Quaternion.identity);
        projetilI.GetComponent<Bolona>().shooter = transform;
    }

    private void dash() {
        dashing = true;
        knockBack((player.position - transform.position).normalized * forcaDash);
    }

    private void setWalkDir() {
        walkDir = (player.position - transform.position).normalized;
    }
    protected override void onDie() {
        PlayerStatus.setProgresso(22);
    }

    protected override void onReverseScale(bool initialScale) {
        if (initialScale) {
            notReverse.localScale = new Vector3(1, 1, 1);
        } else {
            notReverse.localScale = new Vector3(-1, 1, 1);
        }
    }
}

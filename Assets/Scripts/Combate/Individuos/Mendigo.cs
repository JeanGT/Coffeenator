using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mendigo : Inimigo {
    public float timeBetweenAtaques1;
    public float timeBetweenAtaques2;
    public float quantidadeAtaques1;
    public float timeAttacking2;

    public Transform spawnAtaque2;
    public float timeWalking;
    public float timeStopped;
    public Transform[] projetilSpawns1;
    public Transform[] projetilSpawns2;
    public GameObject projetilLento;
    public GameObject projetilRapido;
    public GameObject cachorro;
    public Transform notReverse;

    private float cTimeAttacking2;
    private int cQuantidadeAtaques1;
    private float cTimeBetweenAtaques;
    private bool spawn1 = true;
    private float cTimeWalking;
    private float cTimeStopped;
    private Vector2 walkDir;
    private bool ataqueEmArea;

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
            if (cTimeStopped > timeStopped) {
                if (cTimeWalking > timeWalking) {
                    setWalkDir();
                    attack();
                } else {
                    anim.SetBool("andando", true);
                    rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + walkDir.normalized * cVelocidade * Time.fixedDeltaTime);
                    cTimeWalking += Time.fixedDeltaTime;
                }
            } else {
                anim.SetBool("andando", false);
                cTimeStopped += Time.fixedDeltaTime;
            }
        }
    }

    private void attack() {
        cTimeBetweenAtaques += Time.fixedDeltaTime;
        if (ataqueEmArea) {
            if (cTimeBetweenAtaques > timeBetweenAtaques1) {
                shootAround();
                cTimeBetweenAtaques = 0;
                cQuantidadeAtaques1++;
                if (cQuantidadeAtaques1 == quantidadeAtaques1) {
                    cQuantidadeAtaques1 = 0;
                    terminarAtaque();
                }
            } else {
                anim.SetBool("atacandoParado", true);
            }
        } else {
            setWalkDir();
            rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + walkDir.normalized * cVelocidade * Time.fixedDeltaTime);
            anim.SetBool("atacandoAndando", true);

            cTimeAttacking2 += Time.fixedDeltaTime;
            if (cTimeBetweenAtaques > timeBetweenAtaques2) {
                GameObject projetilI = Instantiate(projetilRapido, spawnAtaque2.position, Quaternion.identity);
                projetilI.GetComponent<Corote>().shooter = transform;
                GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(17);
                cTimeBetweenAtaques = 0;
            }
            if(cTimeAttacking2 > timeAttacking2) {
                cTimeAttacking2 = 0;
                terminarAtaque();
            }
            
        }
        
    }

    private void terminarAtaque() {
        anim.SetBool("atacandoParado", false);
        cTimeWalking = 0;
        cTimeStopped = 0;
        ataqueEmArea = !ataqueEmArea;
        anim.SetBool("atacandoAndando", false);
        anim.SetBool("andando", true);
        //sumonarCachorro();
    }

    private void sumonarCachorro() {
        Instantiate(cachorro, new Vector3(), Quaternion.identity);
    }

    private void shootAround() {
        if (spawn1) {
            for (int i = 0; i < projetilSpawns1.Length; i++) {
                GameObject projetilI = Instantiate(projetilLento, projetilSpawns1[i].position, Quaternion.identity);
                projetilI.GetComponent<Projetil>().shooter = transform;
            }
        } else {
            for (int i = 0; i < projetilSpawns2.Length; i++) {
                GameObject projetilI = Instantiate(projetilLento, projetilSpawns2[i].position, Quaternion.identity);
                projetilI.GetComponent<Projetil>().shooter = transform;
            }
        }
        spawn1 = !spawn1;
    }

    private void setWalkDir() {
        walkDir = (player.position - transform.position).normalized;
    }

    protected override void onReverseScale(bool initialScale) {
        if (initialScale) {
            notReverse.localScale = new Vector3(1, 1, 1);
        } else {
            notReverse.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected override void onDie()
    {
        terminarAtaque();
    }
}

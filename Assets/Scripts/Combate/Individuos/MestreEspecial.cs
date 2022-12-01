using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MestreEspecial : Inimigo {
    public bool doisAtaques;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float timeWalking;
    public float timeStopped;
    public GameObject dashWarning;
    public GameObject lackey;
    public GameObject teleportWarning;
    public int nDashes;
    public int nLacaios;
    public float timeBetweenDashes;
    public float timeUntilReappear;
    public bool avancaObjetivo;

    private bool mostrouWarning;
    private GameObject teleportWarningI;
    private Vector3 teleportPosition;
    private Transform outside;
    private float cTimeUntilReappear;
    private float cTimeBetweenDashes;
    private int cNDashes;

    private bool fumacaSumiuDesaparecer;
    private float cTimeWalking;
    private float cTimeStopped;
    private Vector2 walkDir;
    private bool ataqueNormal = true;
    private GameObject fumacaI2;

    void Start() {
        cVelocidade = velocidade;
        outside = GameObject.Find("Outside").transform;
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
        if (cTimeWalking < timeWalking) {
            anim.SetBool("carregando", false);
            anim.SetBool("reaparecendo", false);
            rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + walkDir * cVelocidade * Time.fixedDeltaTime);
            cTimeWalking += Time.fixedDeltaTime;
        } else {
            if (cTimeStopped > timeStopped) {
                attack();
            } else {
                anim.SetBool("carregando", true);
                cTimeStopped += Time.fixedDeltaTime;
            }
        }
    }

    private void attack() {
        if (!fumacaSumiuDesaparecer) {
            if (fumacaI == null) {
                anim.SetBool("desaparecendo", true);
                jogarFumaca();
            } else if (fumacaI.GetComponent<Fumaca>().desapareceu) {
                fumacaSumiuDesaparecer = true;
                fumacaI.GetComponent<Fumaca>().podeDesaparecer = true;
                transform.position = outside.position;
            }
        }

        if ((cNDashes < nDashes && ataqueNormal) || (cNDashes < nLacaios && !ataqueNormal)) {
            cTimeBetweenDashes += Time.fixedDeltaTime;
            if (cTimeBetweenDashes > timeBetweenDashes) {
                cTimeBetweenDashes = 0;
                cNDashes++;
                if (ataqueNormal) {
                    spawnRandomDashWarning();
                } else {
                    spawnRandomLackey();
                }
            }
        } else {
            cTimeUntilReappear += Time.fixedDeltaTime;
            if (!mostrouWarning) {
                teleportPosition = randomPos();
                teleportWarningI = Instantiate(teleportWarning, teleportPosition, Quaternion.identity);
                mostrouWarning = true;
            }
            if (cTimeUntilReappear > timeUntilReappear) {
                if (fumacaI2 == null) {
                    fumacaI2 = Instantiate(fumaca, teleportPosition, Quaternion.identity);
                } else if (fumacaI2.GetComponent<Fumaca>().desapareceu) {
                    fumacaI2.GetComponent<Fumaca>().podeDesaparecer = true;

                    cTimeWalking = 0;
                    cTimeBetweenDashes = 0;
                    cTimeUntilReappear = 0;
                    fumacaSumiuDesaparecer = false;
                    anim.SetBool("desaparecendo", false);
                    anim.SetBool("reaparecendo", true);
                    cTimeStopped = 0;
                    cNDashes = 0;
                    mostrouWarning = false;
                    transform.position = teleportPosition;
                    Destroy(teleportWarningI);
                    if (doisAtaques) {
                        ataqueNormal = !ataqueNormal;
                    }
                    setWalkDir();
                }
            }
        }
    }



    private void spawnRandomLackey() {
        GameObject lackeyI = Instantiate(lackey, randomPos(), Quaternion.identity);
    }

    private void spawnRandomDashWarning() {
        GameObject projetilI = Instantiate(dashWarning, randomPos(), Quaternion.identity);
        projetilI.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0f, 360f));
    }

    private Vector2 randomPos() {
        return new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
    }

    private void setWalkDir() {
        walkDir = (player.position - transform.position).normalized;
    }

    protected override void onDie()
    {
        if (avancaObjetivo) {
            PlayerStatus.fowardObjective(5, 1);
        }
        PlayerStatus.nivelEspecial++;
    }
}

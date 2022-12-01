using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Orochi : Inimigo {
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float timeWalking;
    public float timeStopped;
    public GameObject dashWarning;
    public GameObject lackey;
    public int nDashes;
    public float timeBetweenDashes;
    public float timeUntilReappear;

    public float timeUnitilTeleport;
    public float timeUntilTargetPlayer;
    public float forcaDash;
    public float timeUntilDash;
    public GameObject teleportWarning;
    public Transform[] projetilSpawns;
    public GameObject projetil;
    public int qSegundoAtaque;
    public int progressoAoDerrotar = -1;
    public float delayTeleporte;

    private float cDelayTeleporte;

    private Vector3 teleportPos;

    private int cQSegundoAtaque;
    private bool targetedPlayer;
    private float cTimeUntilDash;
    private float cTimeUnitilTeleport;
    private Vector3 targetTeleportPos;
    private GameObject spawnedTeleportWarning;

    private GameObject fumacaI2;
    private Transform outside;
    private float cTimeUntilReappear;
    private float cTimeBetweenDashes;
    private int cNDashes;
    private float cTimeWalking;
    private float cTimeStopped;
    private Vector2 walkDir;
    private bool atirou;
    private bool ataqueNormal = true;

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
        if (!isFreezed && !isStunned) {
            if (cTimeWalking < timeWalking) {
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + walkDir.normalized * cVelocidade * Time.fixedDeltaTime);
                cTimeWalking += Time.fixedDeltaTime;
                anim.SetBool("andando", true);
            } else {
                if (cTimeStopped > timeStopped) {
                    attack();
                } else {
                    anim.SetBool("andando", false);
                    anim.SetBool("carregando", true);
                    anim.SetBool("reaparecendo", false);
                    cTimeStopped += Time.fixedDeltaTime;
                }
            }
        }
    }

    private void attack() {
        if (ataqueNormal) {
            if (fumacaI == null) {
                anim.SetBool("carregando", false);
                anim.SetBool("desaparecendo", true);
                jogarFumaca();
            } else if (fumacaI.GetComponent<Fumaca>().desapareceu) {
                fumacaI.GetComponent<Fumaca>().podeDesaparecer = true;
                transform.position = outside.position;
                if (cNDashes < nDashes) {
                    cTimeBetweenDashes += Time.fixedDeltaTime;
                    if (cTimeBetweenDashes > timeBetweenDashes) {
                        cTimeBetweenDashes = 0;
                        cNDashes++;
                        if (Random.value > 0.5f) {
                            spawnRandomDashWarning();
                        } else {
                            spawnRandomLackey();
                        }
                    }
                } else {
                    cTimeUntilReappear += Time.fixedDeltaTime;
                    if (cTimeUntilReappear > timeUntilReappear) {

                        if (fumacaI2 == null) {
                            teleportPos = randomPos();
                            fumacaI2 = Instantiate(fumaca, teleportPos, Quaternion.identity);
                        } else if (fumacaI2.GetComponent<Fumaca>().desapareceu) {
                            fumacaI2.GetComponent<Fumaca>().podeDesaparecer = true;
                            setWalkDir();
                            anim.SetBool("andando", true);
                            anim.SetBool("desaparecendo", false);
                            anim.SetBool("reaparecendo", true);
                            
                            cTimeWalking = 0;
                            cTimeBetweenDashes = 0;
                            cTimeUntilReappear = 0;
                            cTimeStopped = 0;
                            cNDashes = 0;
                            transform.position = teleportPos;
                            ataqueNormal = false;
                        }
                    }
                }
            }


        } else {

            if (cTimeUnitilTeleport > timeUnitilTeleport) {
                if (!targetedPlayer) {
                    anim.SetBool("carregando", false);
                    targetTeleportPos = player.transform.position;
                    spawnedTeleportWarning = Instantiate(teleportWarning, targetTeleportPos, Quaternion.identity);
                    targetedPlayer = true;
                }
                if (cDelayTeleporte < delayTeleporte)
                {
                    cTimeUntilDash += Time.fixedDeltaTime;
                    if (!atirou)
                    {
                        if (fumacaI == null)
                        {
                            jogarFumaca();
                            fumacaI2 = Instantiate(fumaca, targetTeleportPos, Quaternion.identity);
                            fumacaI2.GetComponent<Fumaca>().podeDesaparecer = true;
                            anim.SetBool("desaparecendo", true);
                        }
                        else if (fumacaI.GetComponent<Fumaca>().desapareceu)
                        {
                            anim.SetBool("desaparecendo", false);
                            anim.SetBool("reaparecendo", true);
                            anim.SetBool("carregandoDash", true);
                            fumacaI.GetComponent<Fumaca>().podeDesaparecer = true;
                            transform.position = targetTeleportPos;
                            Destroy(spawnedTeleportWarning);
                            atirou = true;
                            shootAround();
                            cQSegundoAtaque++;
                            if (cQSegundoAtaque == qSegundoAtaque)
                            {
                                ataqueNormal = true;
                                cQSegundoAtaque = 0;
                                cTimeUntilDash = 0;
                                cTimeUnitilTeleport = 0;
                                cDelayTeleporte = 0;
                                targetedPlayer = false;
                                cTimeWalking = 0;
                                cTimeStopped = 0;
                                atirou = false;

                                anim.SetBool("andando", true);
                                anim.SetBool("dash", false);
                                anim.SetBool("carregandoDash", false);
                            }
                        }
                    }
                    if (cTimeUntilDash > timeUntilDash)
                    {
                        anim.SetBool("reaparecendo", false);
                        anim.SetBool("carregandoDash", false);
                        anim.SetBool("dash", true);
                        cTimeUnitilTeleport = 0;
                        cTimeUntilDash = 0;
                        targetedPlayer = false;
                        atirou = false;
                        dash();
                    }
                } else
                {
                    cDelayTeleporte += Time.fixedDeltaTime;
                }
            } else {
                cTimeUnitilTeleport += Time.fixedDeltaTime;
                anim.SetBool("carregando", true);
                anim.SetBool("dash", false);
                anim.SetBool("reaparecendo", false);
            }
        }
    }

    private void shootAround() {
        for (int i = 0; i < projetilSpawns.Length; i++) {
            GameObject projetilI = Instantiate(projetil, projetilSpawns[i].position, Quaternion.identity);
            projetilI.GetComponent<Projetil>().shooter = transform;
        }
    }

    private void dash() {
        dashing = true;
        knockBack((player.position - transform.position).normalized * forcaDash);
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

    protected override void onDie() {
        if (progressoAoDerrotar != -1) {
            PlayerStatus.setProgresso(progressoAoDerrotar);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCombate : Individuo {
    public ShakeBehaviour shakeScreen;

    public static float specialStunnTime = 3f;

    public float timeScaleEspecial;
    public float multiplicadorStatusEspecial;

    public float tempoAteAcabar;

    public Button btnAtacar;
    public Button btnEsquivar;
    public Button btnEspecial;

    public Image vida1;
    public Image vida2;
    public Image vida3;

    public Image especial1;
    public Image especial2;
    public Image especial3;

    public Image esquivarUI;

    public GameObject hitEffect;

    public float precisao;
    public float tempoEntreHits;
    public float timeDashing;
    public float cooldownDash;
    public float velocidadeDash;
    public float velocidadeConsumoEspecial;
    public float quantidadeMaximaEspecial;
    public Transform[] projetilSpawnsDash;

    public GameObject[] projeteis;

    public Animator animPerna;
    public Animator animTronco;

    public Animator morrendo;
    public Animator dash;
    public Animator hit;

    public SpriteRenderer sp1;
    public SpriteRenderer sp2;
    public Text vida;
    public LowLife lowLife;

    public VariableJoystick joystick;

    private bool esquivando;
    private bool atacando;
    private GameObject flash;
    private bool atacando1;
    private GameObject projetil;
    private bool dashing;
    private float cCooldownDash;
    private float cTimeDashing;
    public bool specialAttacking;
    private float qCarregadaEspecial;
    private Vector2 movement;
    private float cTempoEntreHits;
    private bool isLowLife;
    private bool tocouSomDash = false;

    private float initXScale;

    private GameObject inimigo;

    private bool trocouAnimHit;

    void Start() {
        initXScale = sp1.transform.localScale.x;

        projetil = projeteis[PlayerStatus.projetil];

        HP *= PlayerStatus.getVida();
        velocidade *= PlayerStatus.getVelocidade();
        tempoEntreHits /= PlayerStatus.getVelocidadeDeAtaque();

        IndividuoStart();
        cVelocidade = velocidade;
        if (PlayerStatus.nivelEspecial == 0) {
            especial1.gameObject.SetActive(false);
            especial1.gameObject.SetActive(false);
            especial1.gameObject.SetActive(false);
            btnEspecial.gameObject.SetActive(false);
        } else if (PlayerStatus.nivelEspecial > 1) {
            multiplicadorStatusEspecial *= 2;
        }

        hitEffect.SetActive(false);

        
    }

    private void FixedUpdate() {
        IndividuoFixedUpdate();
    }

    protected override void attHealthBar() {
        if (!isLowLife && cHP <= HP * 0.3f)
        {
            isLowLife = true;
            lowLife.lowLife(true);
        }
        float amountVida = cHP / HP;
        vida1.fillAmount = amountVida;
        vida2.fillAmount = amountVida;
        vida3.fillAmount = amountVida;
        vida.text = cHP.ToString("0.0");
    }

    void Update() {
        if (this.animTronco.GetCurrentAnimatorStateInfo(0).IsName("atacando1") || this.animTronco.GetCurrentAnimatorStateInfo(0).IsName("atacando2")) {
            animTronco.SetBool("atacando1", false);
            animTronco.SetBool("atacando2", false);
        }

        if (hitEffect.activeInHierarchy) {
            bool aux = hitEffect.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hit");
            if (!trocouAnimHit) {
                if (aux) {
                    trocouAnimHit = true;
                }
            } else {
                if (aux) {
                    hitEffect.transform.GetChild(0).GetComponent<Animator>().SetBool("hit", false);
                } else {
                    trocouAnimHit = false;
                    hitEffect.SetActive(false);
                }
            }
        }

        if (qCarregadaEspecial < quantidadeMaximaEspecial && !specialAttacking) {
            qCarregadaEspecial += Time.deltaTime;
        }

        float amountEspecial = qCarregadaEspecial / quantidadeMaximaEspecial;
        especial1.fillAmount = amountEspecial;
        especial2.fillAmount = amountEspecial;
        especial3.fillAmount = amountEspecial;

        float amountEsquivar = cCooldownDash / cooldownDash;
        esquivarUI.fillAmount = amountEsquivar;

        IndividuoUpdate();

        if (specialAttacking) {
            qCarregadaEspecial -= Time.deltaTime * velocidadeConsumoEspecial / timeScaleEspecial;
            if (qCarregadaEspecial < 0) {
                stopSpecialAttacking();
            }
        }

        if (dashing) {
            if (movement.x > 0) {
                dash.transform.localScale = new Vector3(initXScale, sp1.transform.localScale.y, sp1.transform.localScale.z);
                dash.transform.localScale = new Vector3(initXScale, sp2.transform.localScale.y, sp2.transform.localScale.z);
            } else {
                dash.transform.localScale = new Vector3(-initXScale, sp1.transform.localScale.y, sp1.transform.localScale.z);
                dash.transform.localScale = new Vector3(-initXScale, sp2.transform.localScale.y, sp2.transform.localScale.z);
            }
        } else if (!isFreezed) {

            inimigo = findClosestEnemy();

            if (inimigo.transform.position.x > transform.position.x) {
                sp1.transform.localScale = new Vector3(initXScale, sp1.transform.localScale.y, sp1.transform.localScale.z);
                sp2.transform.localScale = new Vector3(initXScale, sp2.transform.localScale.y, sp2.transform.localScale.z);
            } else {
                sp1.transform.localScale = new Vector3(-initXScale, sp1.transform.localScale.y, sp1.transform.localScale.z);
                sp2.transform.localScale = new Vector3(-initXScale, sp2.transform.localScale.y, sp2.transform.localScale.z);
            }

            movement = new Vector2(MyInput.getHorizontalAxis(), MyInput.getVerticalAxis()); //wasd

            if (cCooldownDash < cooldownDash) {
                float mult = 1;
                if (specialAttacking)
                {
                    mult = (multiplicadorStatusEspecial / 2) + 0.5f;
                }

                cCooldownDash += Time.deltaTime * mult;
            } else {
                dashing = esquivando && movement != Vector2.zero;
            }

            if (cTempoEntreHits < tempoEntreHits) {
                float mult = 1;
                if (specialAttacking) {
                    mult = (multiplicadorStatusEspecial / 2) + 0.5f;
                }

                cTempoEntreHits += Time.deltaTime * mult;
            } else if (atacando) {
                cTempoEntreHits = 0;
                Transform pivot = GameObject.Find("Ataque Pivot").transform;

                pivot.transform.right = inimigo.transform.position - pivot.transform.position;
                pivot.Rotate(new Vector3(0, 0, Random.Range(-precisao, precisao)));
                atacar(pivot.transform.right);
            }
        }
    }

    public void onRolarFinished() {

    }

    public void onAtacarDown() {
        atacando = true;
    }

    public void onAtacarUp() {
        atacando = false;
    }

    public void onEsquivarDown() {
        esquivando = true;
    }

    public void onEsquivarUp() {
        esquivando = false;
    }

    public void onEspecialDown() {
        if (PlayerStatus.nivelEspecial > 0 && (qCarregadaEspecial >= quantidadeMaximaEspecial || PlayerStatus.nivelEspecial > 2)) {
            Time.timeScale = timeScaleEspecial;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            if (PlayerStatus.nivelEspecial > 2 && specialAttacking) {
                stopSpecialAttacking();
            } else {
                specialAttacking = true;
                dash.SetBool("especial", true);
            }

            GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(10);
        }
    }

    protected override void move() {
        if (!isFreezed) {
            if (dashing) {
                float mul = 1;
                sp1.gameObject.SetActive(false);
                sp2.gameObject.SetActive(false);
                dash.SetBool("dash", true);
                if (specialAttacking) {
                    mul = 1 / timeScaleEspecial;
                }

                if (!tocouSomDash)
                {
                    GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(17);
                    tocouSomDash = true;
                }

                cTimeDashing += Time.fixedDeltaTime * mul;
                if (cTimeDashing > timeDashing) {
                    dash.SetBool("dash", false);
                    sp1.gameObject.SetActive(true);
                    sp2.gameObject.SetActive(true);

                    cTimeDashing = 0;
                    dashing = false;
                    invuneravel = false;
                    cVelocidade = velocidade;
                    cCooldownDash = 0;
                    tocouSomDash = false;

                    if (PlayerStatus.nivelDash > 2) {
                        //atira em volta
                        for (int i = 0; i < projetilSpawnsDash.Length; i++) {
                            GameObject projetilI = Instantiate(projetil, projetilSpawnsDash[i].position, Quaternion.identity);
                            projetilI.GetComponent<Projetil>().shooter = transform;
                            if (PlayerStatus.nivelProjetil > 2) {
                                projetilI.GetComponent<Projetil>().comecarMirando = false;
                            }
                            projetilI.GetComponent<Hurt>().damage *= PlayerStatus.getDano();
                        }
                    }
                } else {
                    if (PlayerStatus.nivelDash > 0) {
                        invuneravel = true;
                    }
                    cVelocidade = velocidadeDash;
                }

            }

            animPerna.SetBool("andando", movement != Vector2.zero);

            float mult = 1;
            if (specialAttacking) {
                mult = multiplicadorStatusEspecial;
            }

            rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + movement.normalized * cVelocidade * Time.fixedDeltaTime * mult);
        }

    }

    protected override void die() {
        sp1.gameObject.SetActive(false);
        sp2.gameObject.SetActive(false);
        morrendo.SetBool("morrendo", true);
        dash.SetBool("dash", false);
        CombateManager.final = inimigo.GetComponent<Inimigo>().final;
        CombateManager.tempoAteAcabar = tempoAteAcabar;
        CombateManager.ganhou = false;
        inimigo.GetComponent<Inimigo>().ficarInvuneravel();

        Destroy(hit.gameObject);
        Destroy(hitEffect.gameObject);
        Destroy(rb);
        Destroy(this.GetComponent<BoxCollider2D>());
        Destroy(this);
    }

    private void stopSpecialAttacking() {
        specialAttacking = false;
        dash.SetBool("especial", false);

        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void atacar(Vector2 dir) {
        if (atacando1) {
            animTronco.SetBool("atacando1", true);
        } else {
            animTronco.SetBool("atacando2", true);
        }

        atacando1 = !atacando1;

        GameObject projetilI = Instantiate(projetil, GameObject.Find("Ataque Position").transform.position, Quaternion.identity);
        projetilI.GetComponent<Projetil>().shooter = transform;
        projetilI.GetComponent<Hurt>().damage *= PlayerStatus.getDano();

        int som = Random.Range(12, 14);
        GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(som);
        cortar(dir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Enemy") && dashing)
        {
            if (PlayerStatus.nivelDash > 0 && cHP > 0)
            {
                sp1.gameObject.SetActive(true);
                sp2.gameObject.SetActive(true);
            } 
            invuneravel = false;
            dash.SetBool("dash", false);

            cTimeDashing = 0;
            dashing = false;

            cVelocidade = velocidade;
            cCooldownDash = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals("Projetil Inimigo"))
        {
            if (PlayerStatus.nivelDash == 0 && dashing)
            {
                dash.SetBool("dash", false);

                if (!isFreezed && cHP > 0)
                {
                    sp1.gameObject.SetActive(true);
                    sp2.gameObject.SetActive(true);
                }

                cTimeDashing = 0;
                dashing = false;

                cVelocidade = velocidade;
                cCooldownDash = 0;
            }
            if (dashing && PlayerStatus.nivelDash > 1 && collision.gameObject.GetComponent<Projetil>().desaparecerAoEncostar)
            {
                GameObject projetilI = Instantiate(projetil, collision.transform.position, Quaternion.identity);
                projetilI.GetComponent<Projetil>().shooter = transform;
                if (PlayerStatus.nivelProjetil > 2)
                {
                    projetilI.GetComponent<Projetil>().comecarMirando = false;
                }
                Destroy(collision.gameObject);
            }
        }
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

    protected override void onGetHit() {
        stopSpecialAttacking();
        shakeScreen.TriggerShake();
    }

    public void onGetHeal()
    {
        lowLife.heal();
        if (cHP > HP * 0.3f)
        {
            isLowLife = false;
            lowLife.lowLife(false);
        }
    }

    public override void knockBack(Vector2 velocity) {
        if (!invuneravel)
        {
            int som = Random.Range(31, 32);
            GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(som);

            hitEffect.SetActive(true);
            hitEffect.transform.GetChild(0).GetComponent<Animator>().SetBool("hit", true);
            hitEffect.transform.position = transform.position;

            sp1.gameObject.SetActive(false);
            sp2.gameObject.SetActive(false);
            hit.SetBool("hit", true);
        }
        base.knockBack(velocity);
    }

    protected override void onUnFreeze() {
        if (cHP > 0)
        {
            sp1.gameObject.SetActive(true);
            sp2.gameObject.SetActive(true);
            hit.SetBool("hit", false);
        }
    }
}

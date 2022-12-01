using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corote : MonoBehaviour
{
    public Transform shooter;
    public float knockBackForce;
    public float forcaInicial;
    public string targetTag;
    public float forcaRecuo;
    public float tempoParado;
    public bool desaparecerAoEncostar = true;
    public float tempoAteDesaparecer = -1;
    public bool isCorote;

    private Rigidbody2D rb;
    private Vector2 dir;
    private bool startExecuted;
    public string shooterTag;
    private bool primeiroUpdate = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startExecuted = true;
        dir = transform.position - shooter.position;
        dir.Normalize();

        shooterTag = shooter.tag;
    }

    void Update()
    {
        if (tempoAteDesaparecer != -1) {
            tempoAteDesaparecer -= Time.deltaTime;
            if (tempoAteDesaparecer < 0) {
                Destroy(this.gameObject);
            }
        }
        if (tempoParado <= 0) {
            if (primeiroUpdate) {
                rb.velocity = dir * forcaInicial;
                primeiroUpdate = false;

                MusicaDeFundo caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
                int som = Random.Range(33, 37);

                if (caixaDeSom != null)
                {
                    caixaDeSom.playSound(som);
                }
            }
            dir = transform.position - shooter.position;
            rb.velocity -= dir * forcaRecuo * Time.deltaTime;
        } else {
            tempoParado -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Enemy") && desaparecerAoEncostar) {
            Destroy(this.gameObject);
        }

        if ((other.tag.Equals("Player") || other.tag.Equals("Wall")) && !other.tag.Equals(shooterTag) && startExecuted) {
            Individuo alvoI = other.gameObject.GetComponent<Individuo>();
            if (alvoI != null) {
                if (knockBackForce != 0) {
                    alvoI.knockBack((other.transform.position - transform.position).normalized * knockBackForce);
                    if (isCorote)
                    {
                        int som = Random.Range(53, 59);
                        GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(som);
                    }
                }

                if (alvoI.isInvuneravel()) {
                    return;
                }
            }
            if (desaparecerAoEncostar) {
                GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(19);
                Destroy(this.gameObject);
            }
        }
    }
}

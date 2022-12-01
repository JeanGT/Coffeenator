using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    public Transform shooter;
    public string targetTag;
    public float velocidade;
    public float knockBackForce;
    public bool teleguiado;
    public float rotateSpeed;
    public bool comecarMirando = true;
    public bool desaparecerAoEncostar = true;
    public float tempoAteDesaparecer = -1;
    public float tempoParado;
    public bool reverse;
    public int somAcertar = -1;

    protected Transform target;
    protected Vector2 dir;
    protected Rigidbody2D rb;
    protected string shooterTag;
    protected bool startExecuted;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = transform.position - shooter.position;
        if (reverse) {
            dir = -dir;
        }
        shooterTag = shooter.tag;

        if (teleguiado)
        {
            target = GameObject.FindGameObjectsWithTag(targetTag)[0].transform;

            Vector3 tDir = target.position - transform.position;

            tDir.Normalize();

            float mult = 1;

            if (comecarMirando)
            {
                if (Mathf.Asin(tDir.y) < 0)
                {
                    mult = -1;
                }
            } else
            {
                if (Mathf.Asin(dir.y) < 0)
                {
                    mult = -1;
                }
            }

            if (comecarMirando) {
                rb.SetRotation(Mathf.Rad2Deg * Mathf.Acos(tDir.x) * mult);
            } else {
                rb.SetRotation(Mathf.Rad2Deg * Mathf.Acos(dir.x) * mult);
            }
        }

        startExecuted = true;
    }

    public void Update() {
        if(tempoAteDesaparecer != -1) {
            tempoAteDesaparecer -= Time.deltaTime;
            if(tempoAteDesaparecer < 0) {
                Destroy(this.gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        if (tempoParado <= 0) {
            if (teleguiado) {
                Vector2 dir = (Vector2)target.position - rb.position; //
                dir.Normalize();
                float rotateAmount = Vector3.Cross(dir, transform.right).z;
                rb.angularVelocity = -rotateAmount * rotateSpeed;
                rb.velocity = transform.right * velocidade;
            } else {
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + dir.normalized * velocidade * Time.fixedDeltaTime);
            }
        } else {
            tempoParado -= Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((other.tag.Equals("Player") || other.tag.Equals("Wall") || other.tag.Equals("Enemy")) && !other.tag.Equals(shooterTag) && startExecuted) {
            Individuo alvoI = other.gameObject.GetComponent<Individuo>();
            if (alvoI != null)
            {
                if (knockBackForce != 0)
                {
                    alvoI.knockBack((other.transform.position - transform.position).normalized * knockBackForce, true);
                }
                if (alvoI.isInvuneravel())
                {
                    return;
                }
            }
            if (desaparecerAoEncostar) {
                GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
                if (caixaDeSom != null && somAcertar != -1)
                {
                    caixaDeSom.GetComponent<MusicaDeFundo>().playSound(somAcertar);
                }

                Destroy(this.gameObject);
            }
        }
    }
}

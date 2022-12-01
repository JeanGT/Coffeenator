using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolona : MonoBehaviour
{
    public Transform shooter;
    public string targetTag;
    public float velocidade;
    public float knockBackForce;
    public float timeBetweenShots;
    public GameObject projetil;

    public Transform[] projetilSpawns;

    private float cTimeBetweenShots;
    protected Transform target;
    protected Vector2 dir;
    protected Rigidbody2D rb;
    protected string shooterTag;
    protected bool startExecuted;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        dir = transform.position - shooter.position;
        transform.right = dir;
        shooterTag = shooter.tag;
        startExecuted = true;
    }

    void Update() {
        cTimeBetweenShots += Time.deltaTime;
        if(cTimeBetweenShots > timeBetweenShots) {
            cTimeBetweenShots = 0;
            shootAround();
        }
    }

    private void shootAround() {
        for (int i = 0; i < projetilSpawns.Length; i++) {
            GameObject projetilI = Instantiate(projetil, projetilSpawns[i].position, Quaternion.identity);
            projetilI.GetComponent<Projetil>().shooter = transform;
        }
    }


    void FixedUpdate() {
         rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + dir.normalized * velocidade * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((other.tag.Equals("Player") || other.tag.Equals("Wall") || other.tag.Equals("Enemy")) && !other.tag.Equals(shooterTag) && startExecuted) {
            Individuo alvoI = other.gameObject.GetComponent<Individuo>();
            if (alvoI != null) {
                if (knockBackForce != 0) {
                    alvoI.knockBack((other.transform.position - transform.position).normalized * knockBackForce);
                }
                if (alvoI.isInvuneravel()) {
                    return;
                }
            }

            Destroy(this.gameObject);
        }
    }
}

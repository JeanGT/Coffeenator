using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Individuo : MonoBehaviour {
    public string nome;
    public float HP;
    public float velocidade;
    public float atrito;
    public float minMagnitude;
    public GameObject corte;
    public GameObject mySprite;

    protected bool invuneravel;
    protected float cHP;
    protected Rigidbody2D rb;
    protected float cVelocidade;
    protected bool isFreezed;
    protected bool isAttacking;
    public bool isDashing;
    protected Animator anim;
    private bool trocouAnimCorte;

    protected void IndividuoStart() {
        if (corte != null) {
            corte.SetActive(false);
        }
        anim = mySprite.GetComponent<Animator>();
        cHP = HP;
        rb = GetComponent<Rigidbody2D>();
        attHealthBar();
    }

    protected void cortar(Vector2 dir) {
        corte.transform.GetChild(0).localScale = new Vector3(corte.transform.GetChild(0).localScale.x, -corte.transform.GetChild(0).localScale.y, corte.transform.GetChild(0).localScale.z);
        corte.SetActive(true);
        corte.transform.GetChild(0).GetComponent<Animator>().SetBool("corte", true);
        corte.transform.right = dir;
    }

    protected void IndividuoFixedUpdate() {
        if (isDashing) { //deslizando
            if (atrito * Time.fixedDeltaTime > rb.velocity.magnitude || rb.velocity.magnitude < minMagnitude) {
                isFreezed = false;
                rb.velocity = Vector2.zero;
                onUnFreeze();
            } else {
                rb.velocity = shortenLength(rb.velocity, atrito * Time.fixedDeltaTime);
                if (rb.velocity.magnitude < minMagnitude) {
                    isFreezed = false;
                    isDashing = false;
                    onUnFreeze();
                    rb.velocity = Vector2.zero;
                }
            }
        }
        move();
    }

    protected void IndividuoUpdate() {
        if (corte != null && corte.activeInHierarchy) {
            bool aux = corte.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("corte");
            if (!trocouAnimCorte) {
                if (aux) {
                    trocouAnimCorte = true;
                }
            } else {
                if (aux) {
                    corte.transform.GetChild(0).GetComponent<Animator>().SetBool("corte", false);
                } else {
                    trocouAnimCorte = false;
                    corte.SetActive(false);
                }
            }
        }
    }

    virtual protected void onUnFreeze() { }

    private Vector2 shortenLength(Vector2 A, float reductionLength) {
        Vector2 B = A;
        B *= (1 - reductionLength / A.magnitude);
        return B;
    }

    abstract protected void move();

    abstract protected void die();

    virtual protected void attHealthBar() {

    }

    public void knockBack(Vector2 velocity, bool a) {
        knockBack(velocity);
    }

    public void hurt(float damage) {
        if (!invuneravel) {
            GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(10);
            onGetHit();
            this.cHP -= damage;
            attHealthBar();
            if (cHP <= 0) {
                cHP = 0;
                die();
            }

        }
    }

    public void heal(float amout) {
        this.cHP += amout;
        if (cHP > HP) {
            cHP = HP;
        }

        attHealthBar();
    }

    public virtual void knockBack(Vector2 velocity) {
        if (!invuneravel) {
            freeze();
            setVelocity(rb.velocity + velocity);
            isDashing = true;
        }
    }

    private void setVelocity(Vector2 velocity) {
        rb.velocity = velocity;
    }

    private void freeze() {
        this.isFreezed = true;
    }

    public void ficarInvuneravel() {
        this.invuneravel = true;
    }

    public void ficarVuneravel() {
        this.invuneravel = false;
    }

    public bool isInvuneravel() {
        return invuneravel;
    }

    protected virtual void onGetHit() {

    }

}

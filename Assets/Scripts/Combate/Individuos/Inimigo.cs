using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class Inimigo : Individuo
{
    public int danoSofridoAoEncostar;
    public float damage;
    public float hitForce;
    public float stunTime;
    public float selfKnockBack;
    public int final;
    public string finalMsg;
    public float tempoAteAcabar;
    public bool isBoss;

    protected bool dashing;

    public Animator hit;

    public GameObject fumaca;

    protected Transform player;

    protected bool isStunned;
    protected float initialXScale;
    private float cXScale;
    protected bool mostrarVida = true;
    protected Slider vidaSlider;
    protected GameObject fumacaI;

    // Start is called before the first frame update
    protected void InimigoStart()
    {
        player = GameObject.Find("Player").transform;
        initialXScale = transform.localScale.x;
        if (mostrarVida) {
            vidaSlider = GameObject.Find("Boss Slider").GetComponent<Slider>();
            vidaSlider.maxValue = HP;
        }
        IndividuoStart();
    }



    protected void InimigoFixedUpdate() {
        IndividuoFixedUpdate();
    }

    // Update is called once per frame
    protected void InimigoUpdate()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            isStunned = true;
        }
        else
        {
            if (isStunned)
            {
                isStunned = false;
            }

            IndividuoUpdate();

            PlayerCombate p = player.GetComponent<PlayerCombate>();
            if (p != null) {
                if (!p.specialAttacking) {
                    if (player.position.x < transform.position.x) {
                        cXScale = initialXScale * -1;
                        onReverseScale(false);
                    } else {
                        cXScale = initialXScale;
                        onReverseScale(true);
                    }
                    transform.localScale = new Vector3(cXScale, transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }

    protected virtual void onReverseScale(bool initialScale) {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb != null)
        {
            isFreezed = false;

            rb.velocity = Vector2.zero;

            if (collision.collider.tag.Equals("Player"))
            {
                Individuo playerI = player.GetComponent<Individuo>();

                playerI.hurt(damage);
                playerI.knockBack((player.position - transform.position).normalized * hitForce);
                dashing = false;
                knockBack((transform.position - collision.gameObject.transform.position).normalized * selfKnockBack);

                hurt(danoSofridoAoEncostar);
            }
            
        }
    }

    protected override void attHealthBar() {
        if (mostrarVida) {
            vidaSlider.value = cHP;
        }
    }

    protected override void die()
    {
        onDie();
        mySprite.SetActive(true);
        hit.SetBool("hit", false);
        anim.SetBool("morrendo", true);

        CombateManager.final = final;
        CombateManager.msgFinal = finalMsg;
        CombateManager.tempoAteAcabar = tempoAteAcabar;
        CombateManager.ganhou = true;
        player.GetComponent<PlayerCombate>().ficarInvuneravel();

        if (isBoss)
        {
            GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(42);
        }
        else
        {
            GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(26);
        }

        GameObject.Find("MusicaBatalha").GetComponent<AudioSource>().Pause();

        rb.velocity = new Vector2();
        Destroy(this.GetComponent<BoxCollider2D>());
        Destroy(this);
    }

    protected abstract void onDie();

    protected void jogarFumaca(Vector3 pos) {
        fumacaI = Instantiate(fumaca, pos, Quaternion.identity);
    }

    protected void jogarFumaca() {
        fumacaI = Instantiate(fumaca, transform.position, Quaternion.identity);
    }

    public override void knockBack(Vector2 velocity) {
        base.knockBack(velocity);

        if (!dashing) {
            mySprite.SetActive(false);
            hit.SetBool("hit", true);
        }
    }

    protected override void onUnFreeze() {
        dashing = false;
        mySprite.SetActive(true);
        hit.SetBool("hit", false);
    }
}

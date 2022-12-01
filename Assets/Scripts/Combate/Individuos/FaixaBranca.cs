using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaixaBranca : Inimigo
{
    public int nDashes;
    public float forcaDash;
    public float timeBetweenDashes;
    public float timeStunned;

    private float cTimeStunned;
    private float cTimeBetweenDashes;
    private float cNDashes;

    void Start()
    {
        InimigoStart();
    }

    private void FixedUpdate() {
        InimigoFixedUpdate();
    }

    void Update()
    {
        InimigoUpdate();
    }

    protected override void move() {
        if (!isFreezed && !isStunned) {
            if (cTimeStunned > timeStunned) {
                if (cNDashes > nDashes) {
                    cTimeStunned = 0;
                    cNDashes = 0;
                    anim.SetBool("dash", false);
                    anim.SetBool("carregandoDash", false);
                } else {
                    if (cTimeBetweenDashes > timeBetweenDashes) {
                            anim.SetBool("dash", true);
                            cNDashes++;
                            cTimeBetweenDashes = 0;
                            dash();
                    } else {
                        cTimeBetweenDashes += Time.fixedDeltaTime;
                        anim.SetBool("carregandoDash", true);
                        anim.SetBool("dash", false);
                    }
                }
            } else {
                cTimeStunned += Time.fixedDeltaTime;
                anim.SetBool("carregandoDash", false);
            }
        }
    }

    private void dash() {
        dashing = true;
        knockBack((player.position - transform.position).normalized * forcaDash);
    }

    protected override void onDie()
    {
        PlayerStatus.nivelDash++;
    }
}

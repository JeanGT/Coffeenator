using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : Bug
{
    public float xChangeOnTeleport;
    public float timeMoving;
    public Animator anim;
    public GameObject ponto;
    public bool boss;

    private float cTimeMoving;

    protected float targetY = -999999;
    protected GameObject spawnedPonto;

    protected override void move() {
        cTimeMoving += Time.deltaTime;
        
        if (cTimeMoving > timeMoving) {
            anim.SetBool("parado", true);
            if(targetY == -999999)
            {
                targetY = Random.Range(minY, maxY);
                if(ponto != null)
                spawnedPonto = Instantiate(ponto, new Vector3(transform.position.x, targetY, transform.position.z), Quaternion.identity);
            }
            if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Glitch") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) || (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))) {
                anim.SetBool("parado", false);
                cTimeMoving = 0;
                usarHabilidade();
                targetY = -999999;
            }
        } else {
            base.move();
        }
    }

    protected override void onDie()
    {
        if(spawnedPonto != null)
        Destroy(spawnedPonto);
    }

    protected virtual void usarHabilidade() {
        transform.position = new Vector3(transform.position.x + xChangeOnTeleport, targetY, transform.position.z);
        ProgramarManager.caixaDeSom.playSound(61);
        Destroy(spawnedPonto);
    } 


}

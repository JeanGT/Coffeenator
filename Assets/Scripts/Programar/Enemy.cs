using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float maxHP;
    public int damage;

    protected float cHP;
    private GameObject canhao;

    // Start is called before the first frame update
    protected void EnemyStart()
    {
        cHP = maxHP;
        canhao = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected void EnemyUpdate()
    {
        move();
        if(transform.position.x < canhao.transform.position.x){
            hit();
        }
    }


    void OnTriggerEnter2D(Collider2D col) {
        hurt(col.GetComponent<ProjetilProgramar>().damage);
        col.GetComponent<ProjetilProgramar>().onHit();
    }

    public void hurt(float damage) {
        cHP -= damage;
        if (cHP <= 0) {
            die();
        }
        onGetHurt();
    }

    abstract protected void die();
    abstract protected void onDie();
    abstract protected void move();
    abstract protected void hit();

    protected virtual void onGetHurt() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aliado : MonoBehaviour
{
    public float velocidade;
    public float distanciaMinima;

    private Transform player;
    private float initXScale;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        initXScale = transform.localScale.x;
        player = GameObject.Find("Player").transform;
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
    }

    private void move() {
        bool correndo = false;
        if (transform.position.x > player.position.x) { //andar pra esquerda
            transform.localScale = new Vector3(-initXScale, transform.localScale.y, transform.localScale.z);
            if(transform.position.x - player.position.x > distanciaMinima) {
                transform.position += new Vector3(-velocidade * Time.fixedDeltaTime, 0, 0);
                correndo = true;
            }
        } else { //andar pra direita
            transform.localScale = new Vector3(initXScale, transform.localScale.y, transform.localScale.z);
            if (player.position.x - transform.position.x > distanciaMinima) {
                transform.position += new Vector3(velocidade * Time.fixedDeltaTime, 0, 0);
                correndo = true;
            }
        }
        anim.SetBool("correndo", correndo);
    }
}

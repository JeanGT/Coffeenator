using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cachorro : Inimigo
{
    public Transform notReverse;
    public float sensibilidade;
    public float reloadVirar;

    private float cReloadVirar;

    void Start() {
        cVelocidade = velocidade;
        InimigoStart();
    }

    private void FixedUpdate() {
        InimigoFixedUpdate();
    }

    // Update is called once per frame
    void Update() {
        InimigoUpdate();
    }

    protected override void move() {
        Vector2 dir = new Vector2();
        if (Mathf.Abs(player.position.x - transform.position.x) > sensibilidade) {
            if (player.position.x > transform.position.x) {
                dir = Vector2.right;
            } else {
                dir = Vector2.left;
            }
        } else {
            if (player.position.y > transform.position.y) {
                dir = Vector2.up;
            } else {
                dir = Vector2.down;
            }
        }

        transform.right = dir;
        rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + dir * cVelocidade * Time.fixedDeltaTime);
    }
    protected override void die() {
        Destroy(this.gameObject);
    }

    protected override void onDie() {
        
    }

    protected override void onReverseScale(bool initialScale) {
        if (initialScale) {
            notReverse.localScale = new Vector3(1, 1, 1);
        } else {
            notReverse.localScale = new Vector3(-1, 1, 1);
        }
    }

}

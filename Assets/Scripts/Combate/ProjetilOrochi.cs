using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilOrochi : Projetil
{
    // Start is called before the first frame update
    void Start()
    {
        shooterTag = shooter.tag;
        startExecuted = true;
    }


    void FixedUpdate() {
        dir = transform.position - shooter.position;
        transform.position = new Vector2(transform.position.x, transform.position.y) + dir.normalized * velocidade * Time.fixedDeltaTime;
    }
}

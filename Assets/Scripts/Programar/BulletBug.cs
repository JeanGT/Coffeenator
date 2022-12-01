using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBug : Bug {
    Vector2 dir;
    public float precisao;

    void Start() {
        EnemyStart();

        Vector3 targetPos = new Vector2(GameObject.Find("Player").transform.position.x, Random.Range(minY, maxY));

        dir = targetPos - transform.position;
        dir.Normalize();
        transform.right = dir;
    }

    protected override void move() {
        transform.position += new Vector3(dir.x * xVelocity * Time.deltaTime, dir.y * xVelocity * Time.deltaTime, 0);
    }
}

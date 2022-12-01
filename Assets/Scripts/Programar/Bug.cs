using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bug : Enemy
{
    public GameObject explosao;
    public float xVelocity;
    public float yVelocity;
    public float maxY;
    public float minY;

    private int direction = 1;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        EnemyStart();
        BugStart();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyUpdate();
    }

    protected override void die() {
        ProgramarManager.caixaDeSom.playSound(16);
        GameObject.Find("BugSpawn").GetComponent<SpawnBug>().removeBug(index);
        Instantiate(explosao, transform.position, Quaternion.identity);
        onDie();
        Destroy(this.gameObject);
    }

    protected override void onDie() { }

    protected override void move() {
        transform.position = new Vector3(transform.position.x + -xVelocity * Time.deltaTime, transform.position.y + yVelocity * Time.deltaTime * direction, transform.position.z);
        if(transform.position.y > maxY) {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            direction *= -1;
        } else if (transform.position.y < minY) {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
            direction *= -1;
        }
    }

    protected override void hit() {
        GameObject.Find("Player").GetComponent<PlayerProgramar>().hit(damage);
        die();
    }

    public void setIndex(int index) {
        this.index = index;
    }

    protected virtual void BugStart() {
        if (Random.Range(0f, 1f) > 0.5f)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }
}

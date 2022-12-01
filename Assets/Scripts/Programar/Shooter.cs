using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : Glitch
{
    public GameObject bullet;
    public Slider barraDeVida;
    public float distanciaSpawnX;
    public float distanciaY;
    public float xStartOffSet;
    void Start() {
        EnemyStart();
        transform.position += new Vector3(xStartOffSet, 0, 0);
    }
    protected override void BugStart()
    {
        base.BugStart();
        barraDeVida.value = barraDeVida.maxValue;
    }

    protected override void usarHabilidade() {
        GameObject tiro1 = (GameObject)Instantiate(bullet, new Vector3(transform.position.x - distanciaSpawnX, transform.position.y + distanciaY, transform.position.z), Quaternion.identity);
        GameObject tiro2 = (GameObject)Instantiate(bullet, new Vector3(transform.position.x - distanciaSpawnX, transform.position.y - distanciaY, transform.position.z), Quaternion.identity);

        ProgramarManager.caixaDeSom.playSound(74);

        SpawnBug spawn = GameObject.Find("BugSpawn").GetComponent<SpawnBug>();
        spawn.addBug(tiro1);
        spawn.addBug(tiro2);
    }
    protected override void onGetHurt()
    {
        anim.SetBool("hit", true);

        if (barraDeVida != null) {
            barraDeVida.value = cHP;
        }
    }

    protected override void onDie()
    {
        if (boss)
        {
            ProgramarManager.caixaDeSom.playSound(16);
        }
    }

    public void desativarAnimacao()
    {
        anim.SetBool("hit", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Summoner : Glitch
{
    public GameObject minion;
    public float distanciaSpawn;
    public Slider barraDeVida;

    protected override void BugStart()
    {
        base.BugStart();
        barraDeVida.value = barraDeVida.maxValue;
    }

    protected override void usarHabilidade() {
        ProgramarManager.caixaDeSom.playSound(60);

        SpawnBug spawn = GameObject.Find("BugSpawn").GetComponent<SpawnBug>();

        if (transform.position.y + distanciaSpawn < maxY)
        {
            GameObject bugCima = (GameObject)Instantiate(minion, new Vector3(transform.position.x, transform.position.y + distanciaSpawn, transform.position.z), Quaternion.identity);
            spawn.addBug(bugCima);
        }
        if (transform.position.y - distanciaSpawn > minY)
        {
            GameObject bugBaixo = (GameObject)Instantiate(minion, new Vector3(transform.position.x, transform.position.y - distanciaSpawn, transform.position.z), Quaternion.identity);
            spawn.addBug(bugBaixo);
        }
        if (!boss) {
            GameObject bugDireita = (GameObject)Instantiate(minion, new Vector3(transform.position.x + distanciaSpawn, transform.position.y, transform.position.z), Quaternion.identity);
            spawn.addBug(bugDireita);
            GameObject bugEsquerda = (GameObject)Instantiate(minion, new Vector3(transform.position.x - distanciaSpawn, transform.position.y, transform.position.z), Quaternion.identity);
            spawn.addBug(bugEsquerda);
        }
    }

    protected override void onGetHurt()
    {
        anim.SetBool("hit", true);
        barraDeVida.value = cHP;
    }
    public void desativarAnimacao()
    {
        anim.SetBool("hit", false);
    }

}

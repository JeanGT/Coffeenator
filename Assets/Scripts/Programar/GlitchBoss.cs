using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlitchBoss : Glitch
{
    public GameObject minion;
    public Slider barraDeVida;
    protected override void BugStart()
    {
        base.BugStart();
        barraDeVida.value = barraDeVida.maxValue;
    }

    protected override void usarHabilidade()
    {
        SpawnBug spawn = GameObject.Find("BugSpawn").GetComponent<SpawnBug>();

        GameObject bugMinion = (GameObject)Instantiate(minion, new Vector3(transform.position.x, targetY, transform.position.z), Quaternion.identity);
        spawn.addBug(bugMinion);
        ProgramarManager.caixaDeSom.playSound(61);
        Destroy(spawnedPonto);
    }
    protected override void onGetHurt()
    {
        barraDeVida.value = cHP;
    }
}

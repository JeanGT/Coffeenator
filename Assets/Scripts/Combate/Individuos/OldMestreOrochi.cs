using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMestreOrochi : Inimigo
{
    public float timeUnitilTeleport;
    public float timeUntilTargetPlayer;
    public float forcaDash;
    public float timeUntilDash;
    public GameObject teleportWarning;
    public Transform[] projetilSpawns;
    public GameObject projetil;

    private bool targetedPlayer;
    private float cTimeUntilDash;
    private float cTimeUnitilTeleport;
    private Vector3 targetTeleportPos;
    private GameObject spawnedTeleportWarning;


    void Start() {
        InimigoStart();
    }

    void Update() {
        InimigoUpdate();
    }

    private void FixedUpdate() {
        InimigoFixedUpdate();
    }

    protected override void move() {
        cTimeUnitilTeleport += Time.fixedDeltaTime;
        if (cTimeUnitilTeleport > timeUntilTargetPlayer && !targetedPlayer) {
            targetTeleportPos = player.transform.position;
            spawnedTeleportWarning = Instantiate(teleportWarning, targetTeleportPos, Quaternion.identity);
            targetedPlayer = true;
        }
        if (cTimeUnitilTeleport > timeUnitilTeleport) {
            transform.position = targetTeleportPos;
            Destroy(spawnedTeleportWarning);
            cTimeUntilDash += Time.fixedDeltaTime;
            if (cTimeUntilDash > timeUntilDash) {
                cTimeUnitilTeleport = 0;
                cTimeUntilDash = 0;
                targetedPlayer = false;
                dash();
            }
        }
    }

   // protected override void onUnFreeze() {
    //    attack();
   // }

    private void dash() {
        knockBack((player.position - transform.position).normalized * forcaDash);
    }

    private void attack() {
        for (int i = 0; i < projetilSpawns.Length; i++) {
            GameObject projetilI = Instantiate(projetil, projetilSpawns[i].position, Quaternion.identity);
            projetilI.GetComponent<Projetil>().shooter = transform;
        }
    }

    protected override void onDie()
    {
       
    }
}

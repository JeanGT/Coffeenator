using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaixaAzul : Inimigo
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float precisaoTarget;
    public float timeUnitilTeleport;
    public float timeUntilTargetPlayer;
    public float forcaDash;
    public float timeUntilDash;
    public GameObject teleportWarning;

    private bool targetedPlayer;
    private float cTimeUntilDash;
    private float cTimeUnitilTeleport;
    private Vector3 targetTeleportPos;
    private GameObject spawnedTeleportWarning;
    private bool jogouFumaca;
    private bool teleportar;
    public bool avancaObjetivo;

    void Start()
    {
        InimigoStart();
    }

    void Update()
    {
        InimigoUpdate();
    }

    private void FixedUpdate() {
        InimigoFixedUpdate();
    }

    protected override void move() {
        if (!isFreezed && !isStunned)
        {
            if (cTimeUnitilTeleport > timeUntilTargetPlayer && !targetedPlayer)
            {
                anim.SetBool("carregando", true);
                targetTeleportPos = player.transform.position;
                float newX = Random.Range(-precisaoTarget, precisaoTarget);
                float newY = Random.Range(-precisaoTarget, precisaoTarget);
                if (targetTeleportPos.x + newX > maxX || targetTeleportPos.x + newX < minX)
                {
                    newX *= -1;
                }
                if (targetTeleportPos.y + newY > maxY || targetTeleportPos.y + newY < minY)
                {
                    newY *= -1;
                }
                targetTeleportPos = new Vector3(targetTeleportPos.x + newX, targetTeleportPos.y + newY, targetTeleportPos.z);
                spawnedTeleportWarning = Instantiate(teleportWarning, targetTeleportPos, Quaternion.identity);
                targetedPlayer = true;
            } else {
                anim.SetBool("carregando", false);
                anim.SetBool("dash", false);
                cTimeUnitilTeleport += Time.fixedDeltaTime;
            }

            if (teleportar) {
                if (cTimeUntilDash > timeUntilDash) {
                    anim.SetBool("carregandoDash", false);
                    anim.SetBool("dash", true);
                    cTimeUnitilTeleport = 0;
                    teleportar = false;
                    jogouFumaca = false;
                    cTimeUntilDash = 0;
                    targetedPlayer = false;
                    dash();
                } else {
                    anim.SetBool("carregandoDash", true);
                    cTimeUntilDash += Time.fixedDeltaTime;
                }
            } else {
                if (cTimeUnitilTeleport > timeUnitilTeleport) {
                    if (!jogouFumaca) {
                        jogouFumaca = true;
                        jogarFumaca();
                        GameObject fumacaI2 = Instantiate(fumaca, targetTeleportPos, Quaternion.identity);
                        fumacaI2.GetComponent<Fumaca>().podeDesaparecer = true;
                        anim.SetBool("teleportando", true);
                    } else if (fumacaI.GetComponent<Fumaca>().desapareceu) {
                        fumacaI.GetComponent<Fumaca>().podeDesaparecer = true;
                        teleportar = true;
                        transform.position = targetTeleportPos;
                        Destroy(spawnedTeleportWarning);
                    }
                } else {
                    anim.SetBool("teleportando", false);
                }
            }
        }
    }

    private void dash() {
        dashing = true;
        knockBack((player.position - transform.position).normalized * forcaDash);
    }

    protected override void onDie()
    {
        if (avancaObjetivo)
        {
            PlayerStatus.fowardObjective(5, 1);
        }
        PlayerStatus.nivelDash++;
    }
}

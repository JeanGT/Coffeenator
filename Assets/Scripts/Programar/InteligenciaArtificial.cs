using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteligenciaArtificial : MonoBehaviour
{
    public float damage;
    public float reloadTime;
    public float timeBetweenShots;
    public GameObject laser;
    public float tempoAtacando;

    private bool atacou;
    private float cReloadTime;
    private float cTempoAtacando;
    private float timeSinceLastShot;
    private SpriteRenderer spLaser;
    private Vector2 direction;
    private GameObject lastBug;

    // Start is called before the first frame update
    void Start() {
        if (!PlayerStatus.getInteligencia()[3]) {
            gameObject.SetActive(false);
        } else if (PlayerStatus.getInteligencia()[4])
        {
            reloadTime /= 2;
            timeBetweenShots /= 2;
            damage *= 2;
        }
        direction = new Vector2();
        timeSinceLastShot = timeBetweenShots;
        spLaser = laser.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (cReloadTime < 0) {
            if (lastBug == null)
            {
                lastBug = GameObject.Find("BugSpawn").GetComponent<SpawnBug>().getRandomBug();
            }

            if (lastBug != null) {
                if (timeSinceLastShot > timeBetweenShots) {
                    spLaser.color = new Color(0, 1, 0, 1);
                    cTempoAtacando += Time.deltaTime;
                    if (!atacou) {
                        lastBug.GetComponent<Enemy>().hurt(damage);
                        atacou = true;
                    }

                    if (cTempoAtacando > tempoAtacando) {
                        timeSinceLastShot = 0;
                        cTempoAtacando = 0;
                        cReloadTime = reloadTime;
                        ProgramarManager.caixaDeSom.playSound(72);
                        atacou = false;
                        spLaser.color = new Color(1, 0, 0, 0);
                        lastBug = null;
                    }
                } else {
                    spLaser.color = new Color(1, 0, 0, ((timeSinceLastShot) / timeBetweenShots));
                    timeSinceLastShot += Time.deltaTime;
                    Vector3 targetPosition = lastBug.transform.position;
                    direction = (Vector2)((targetPosition - transform.position));
                    direction.Normalize();
                    transform.right = direction;
                }
            } else {
                spLaser.color = new Color(0, 0, 0, 0);
                cTempoAtacando = 0;
                timeSinceLastShot = 0;
            }
        } else {
            cReloadTime -= Time.deltaTime;
        }
    }
}

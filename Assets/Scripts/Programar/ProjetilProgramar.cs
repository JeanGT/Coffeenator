using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilProgramar : MonoBehaviour
{
    public int damage;

    public Sprite sprite1;
    public Sprite sprite2;

    public float timeUntilDestroy;
    public float maxX;

    public bool destroyOnHit;

    void Start() {
        SpriteRenderer spriteR = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        if (spriteR != null) {
            Sprite sprite;
            if (Random.Range(0f, 1f) > 0.5f) {
                sprite = sprite1;
            } else {
                sprite = sprite2;
            }
            spriteR.sprite = sprite;
        }

        ParticleSystem particulas = this.GetComponent<ParticleSystem>();

        if (particulas != null)
        {
            if (damage != 1)
            {
                particulas.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                particulas.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    void Update()
    {
        timeUntilDestroy -= Time.deltaTime;
        if (timeUntilDestroy <= 0 || transform.position.x > maxX)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void onHit() {
        if (destroyOnHit)
        {
            Destroy(this.gameObject);
        }
    }
}

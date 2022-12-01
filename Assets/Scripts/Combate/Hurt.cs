using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt : MonoBehaviour
{
    public string targetTag;
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(targetTag)) {
            collision.GetComponent<Individuo>().hurt(damage);
        }
    }
}

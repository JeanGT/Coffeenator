using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScale : MonoBehaviour
{
    public float velocidade;
    public float targetScale;

    private float initialScale;
    private bool increasing;

    void Start()
    {
        initialScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (increasing) {
            if(transform.localScale.x < targetScale) {
                float aux = velocidade * Time.deltaTime;
                transform.localScale += new Vector3(aux, aux, 0);
            } else {
                transform.localScale = new Vector3(targetScale, targetScale, 1);
            }
        } else {
            if (transform.localScale.x > initialScale) {
                float aux = velocidade * Time.deltaTime;
                transform.localScale -= new Vector3(aux, aux, 0);
            } else {
                transform.localScale = new Vector3(initialScale, initialScale, 1);
            }
        }
    }

    public void increase() {
        increasing = true;
    }

    public void goBack() {
        MusicaDeFundo caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
        if (caixaDeSom != null)
            caixaDeSom.playSound(17);

        increasing = false;
    }

    public bool isInitialScale() {
        return transform.localScale.x == initialScale;
    }
}

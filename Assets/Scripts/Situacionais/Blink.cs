using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public float velocidade;

    private Image img;
    private bool blinking;
    private float progress;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (blinking)
        {
            progress += velocidade * Time.deltaTime;
            float a = Mathf.Sin(progress);
            img.color = new Color(img.color.r, img.color.g, img.color.b, a);
            if (progress >= 3.1415f)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
                blinking = false;
            }
        }
    }

    public void blink()
    {
        blinking = true;
    }

    public void resetBlink()
    {
        blinking = false;
        progress = 0;
    }
}

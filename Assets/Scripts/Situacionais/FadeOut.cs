using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float velocidade;
    public int startProgress;

    private Image img;
    private bool faddingOut;

    void Start()
    {
        img = GetComponent<Image>();

        if (PlayerStatus.getProgresso() == startProgress)
        {
            fadeOut();
        } else {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        }

    }

    void Update()
    {
        if (faddingOut) {
            float a = img.color.a - velocidade * Time.deltaTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b, a);
            if(a <= 0.4f)
            {
                gameObject.SetActive(false);
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
            }
        }
    }

    public void fadeOut() {
        faddingOut = true;
    }
}

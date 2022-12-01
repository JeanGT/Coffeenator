using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float velocidade;

    private Image img;
    private Text txt;
    private bool faddingIn;

    void Start() {
        img = GetComponent<Image>();
        if(img == null) {
            txt = GetComponent<Text>();
        }
    }

    void Update() {
        if (faddingIn) {
            float a;
            if (img != null) {
                a = img.color.a + velocidade * Time.deltaTime;
                img.color = new Color(img.color.r, img.color.g, img.color.b, a);
            } else {
                a = txt.color.a + velocidade * Time.deltaTime;
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, a);
            }
            if (a > 1) {
                if (img != null) {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
                } else {
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
                }
            }
        }
    }

    public void fadeIn() {
        faddingIn = true;
    }
}

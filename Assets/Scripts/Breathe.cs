using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breathe : MonoBehaviour
{
    public float max;
    public float min;
    public float speed;
    public bool mudarOpacidadeImagem = false;

    private Text text;
    private Image image;
    private Button btn;

    void Start()
    {
        text = GetComponent<Text>();
        image = GetComponent<Image>();
        btn = GetComponent<Button>();
    }

    void Update()
    {
        if(btn != null)
        {
            if (!btn.interactable)
            {
                image.color = new Color(1, 1, 1, 1);
                return;
            }
        }

        float opacidade = Mathf.Cos(Time.unscaledTime * speed);
        opacidade =  (opacidade + 1) / 2 * (max - min) + min;

        if (text !=  null)
            text.color = new Color(text.color.r, text.color.g, text.color.b, opacidade);

        if (image != null)
        {
            if (mudarOpacidadeImagem)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, opacidade);
            }
            else
            {
                image.color = new Color(image.color.r, opacidade, opacidade, image.color.a);
            }
        }
    }
}

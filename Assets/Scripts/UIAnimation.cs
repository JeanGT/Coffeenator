using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    public float velocidade;
    public Sprite[] sprites;

    private Image image;
    private int index = 0;
    private float timer = 0;

    void Start()
    {
        image = this. GetComponent<Image>();
    }

    void Update()
    {
        if ((timer += Time.deltaTime) >= (velocidade))
        {
            timer = 0;
            image.sprite = sprites[index];
            index = (index + 1) % sprites.Length;
        }
    }
}

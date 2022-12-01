using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAlphaTextButton : MonoBehaviour
{
    public Text text;
    public Button btn;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float a;
        if (btn.interactable) {
            a = 1;
        } else {
            a = btn.colors.disabledColor.a;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, a);
    }
}

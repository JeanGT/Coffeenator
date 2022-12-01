using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowLife : MonoBehaviour
{
    public float durationHeal;
    public float breathVelocity;

    private Image image;
    private float cDurantionHeal;
    private Vector3 color;
    private bool isLowLife;
    private float breath;

    void Start()
    {
        image = GetComponent<Image>();
        cDurantionHeal = durationHeal;
    }

    void Update()
    {
        if (isLowLife)
        {
            breath += Time.deltaTime * breathVelocity;
            float a = Mathf.Abs(Mathf.Sin(breath));
            image.color = new Color(1, 0, 0, (a / 2) + 0.5f);
        } else if (cDurantionHeal < durationHeal)
        {
            cDurantionHeal += Time.deltaTime;
            breath += Time.deltaTime * breathVelocity;
            float a = Mathf.Sin(breath);
            image.color = new Color(0, 1, 0, a);
        }
        else
        {
            image.color = new Color(0, 0, 0, 0);
        }
    }

    public void lowLife(bool status)
    {
        isLowLife = status;
        breath = 0;
        image.color = new Color(1, 0, 0, 0);
    }

    public void heal()
    {
        if (!isLowLife)
        {
            cDurantionHeal = 0;
            breath = 0;
            image.color = new Color(1, 0, 0, 0);
        }
    }
}

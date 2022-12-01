using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosteCreditos : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D luz;
    private CreditosManager sol;

    void Start()
    {
        luz = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        luz.enabled = false;

        sol = GameObject.Find("Main Camera").GetComponent<CreditosManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sol.chegou)
        {
            StartCoroutine(acenderLuz());
        }
        else
        {
            luz.enabled = false;
        }
    }

    IEnumerator acenderLuz()
    {
        yield return new WaitForSeconds(2);

        luz.enabled = true;
    }
}

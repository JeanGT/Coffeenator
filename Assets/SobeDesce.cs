using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SobeDesce : MonoBehaviour
{
    public float velocidadeX;
    public float velocidadeDeVariacao;
    public float variacao;
    public float limiteX;

    private float yInicial;

    void Start()
    {
        yInicial = transform.position.y;
    }

    void Update()
    {
        float y = yInicial + Mathf.Cos(Time.time * velocidadeDeVariacao) * variacao;

        transform.position = new Vector3(transform.position.x + Time.deltaTime * velocidadeX, y, transform.position.z);

        if(transform.position.x > limiteX)
        {
            Destroy(this.gameObject);
        }
    }
}

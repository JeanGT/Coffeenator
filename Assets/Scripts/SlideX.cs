using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideX : MonoBehaviour
{
    public float velociade;

    private float distanciaMovimento;
    private RectTransform t;
    private float initialX;
    private bool movendo = false;
    private int direcao = -1; //1 = direita

    // Start is called before the first frame update
    void Awake()
    {
        initialX = transform.position.x;

        t = GetComponent<RectTransform>();

        distanciaMovimento = t.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (movendo) {
            transform.position = new Vector3(transform.position.x + velociade * Time.deltaTime * direcao, transform.position.y, transform.position.z);
            if(transform.position.x <= initialX && direcao == -1) {
                movendo = false;
                transform.position = new Vector3(initialX, transform.position.y, transform.position.z);
            } else if(transform.position.x >= initialX + distanciaMovimento && direcao == 1) {
                movendo = false;
                transform.position = new Vector3(initialX + distanciaMovimento, transform.position.y, transform.position.z);
            }
        }
    }

    public void mover() {
        movendo = true;
        direcao *= -1;
        GameManager.objetivosAberto = !GameManager.objetivosAberto;
    }

    public void abrirInstantaneo() {
        transform.position = new Vector3(initialX, transform.position.y, transform.position.z);
        direcao = -1;
        movendo = false;
    }

    public void fecharInstantaneo() {
        transform.position = new Vector3(initialX + distanciaMovimento, transform.position.y, transform.position.z);
        direcao = 1;
        movendo = false;
    }
}

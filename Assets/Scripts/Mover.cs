using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public int progresso;
    public int progressoFinal = -1;
    public float destino;
    public float velocidade;
    public string parametroAnimacao;

    private int direcao;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (destino - transform.position.x > 0) {
            direcao = 1;
        } else {
            direcao = -1;
        }

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(PlayerStatus.getProgresso() == progresso) {
            transform.position += new Vector3(velocidade * direcao * Time.fixedDeltaTime, 0, 0);
            animator.SetBool(parametroAnimacao, true);
            if ((transform.position.x >= destino && direcao == 1) || (transform.position.x <= destino && direcao == -1)) {
                animator.SetBool(parametroAnimacao, false);
                if (progressoFinal != -1) {
                    PlayerStatus.setProgresso(progressoFinal);
                }
            }
        }
        
    }
}

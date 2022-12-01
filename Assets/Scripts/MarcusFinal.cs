using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcusFinal : MonoBehaviour
{
    public float velocidade;
    public float targetX;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(PlayerStatus.getProgresso() == 28) {
            if(transform.position.x > targetX) {
                anim.SetBool("andando", false);
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
                PlayerStatus.setProgresso(29);
                LookAt.setTargetName("Marcus");
            } else {
                anim.SetBool("andando", true);
                transform.position += new Vector3(velocidade * Time.deltaTime, 0, 0);
            }
        }
    }
}

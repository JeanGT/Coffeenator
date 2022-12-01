using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fumaca : MonoBehaviour
{
    private Animator anim;

    public bool desapareceu;
    public bool podeDesaparecer;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("destruir")) {
            Destroy(this.gameObject);
        }
    }

    public void onDesaparecer()
    {
        desapareceu = true;
    }
    
}

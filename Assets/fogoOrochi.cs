using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogoOrochi : MonoBehaviour
{
    public Sprite spriteMovendo;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Rigidbody2D>().velocity != Vector2.zero) {
            GetComponent<SpriteRenderer>().sprite = spriteMovendo;
        }
    }
}

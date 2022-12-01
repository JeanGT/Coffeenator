using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcionaCabana : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (PlayerStatus.getAvisoMendigo() == 0 && PlayerStatus.getProgresso() < 13 && PlayerStatus.getProgresso() > 6)
        {
            PlayerStatus.setAvisoMendigo(1);
        }
    }
}
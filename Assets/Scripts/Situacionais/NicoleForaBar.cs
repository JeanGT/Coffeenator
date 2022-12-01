using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicoleForaBar : MonoBehaviour
{
    public int nivelMinimoDash = 1;

    void Start()
    {
        if(PlayerStatus.nivelDash < nivelMinimoDash || PlayerStatus.getProgresso() > 12) {
            Destroy(this.gameObject);
        }
    }
}

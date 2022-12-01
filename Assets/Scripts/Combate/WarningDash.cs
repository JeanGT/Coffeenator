using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningDash : MonoBehaviour
{
    public float timeUntilDisappear;
    public Projetil projetil;

    private float cTimeUntilDisappear;
    private bool acabou;
    private bool destroyOnNext;

    void Update()
    {
        cTimeUntilDisappear += Time.deltaTime;
        if(cTimeUntilDisappear > timeUntilDisappear) {
            Destroy(this.gameObject);
        }
    }
}

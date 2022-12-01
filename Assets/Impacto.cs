using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impacto : MonoBehaviour
{
    public float timeUntilDesapear;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilDesapear -= Time.deltaTime;
        if(timeUntilDesapear < 0) {
            Destroy(this.gameObject);
        }
    }
}

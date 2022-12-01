using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivadorDeAliado : MonoBehaviour
{
    public float yOffSet;

    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ativarNicole() {
        PlayerStatus.ativarNicole();
        GameObject nicole = (GameObject)Instantiate(prefab, transform.position + new Vector3(0, yOffSet, 0), Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void ativarAlex() {
        PlayerStatus.ativarAlex();
        GameObject alex = (GameObject)Instantiate(prefab, transform.position + new Vector3(0, yOffSet, 0), Quaternion.identity);
        Destroy(this.gameObject);
    }
}

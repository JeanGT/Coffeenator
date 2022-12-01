using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objeto : MonoBehaviour {

    [SerializeField] protected string tituloInteracao;

    // Use this for initialization
    protected void StartObjeto () {
		
	}

    // Update is called once per frame
    protected void UpdateObjeto () {
		
	}

    abstract public void interagir();

    public string getTituloInteracao() {
        return this.tituloInteracao;
    }
}

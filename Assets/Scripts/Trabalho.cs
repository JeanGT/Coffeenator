using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Novo Trabalho", menuName = "Trabalho")]
public class Trabalho : ScriptableObject
{ 
    public bool java;
    public bool python;
    public bool javascript;
    public float salario;
    public float velocidade;
    public int inimigos;
    public string titulo;
    public string descricao;
    public Sprite cliente;
    public bool tutorial = false;
}

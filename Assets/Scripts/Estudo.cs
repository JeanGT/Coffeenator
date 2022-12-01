using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Novo Estudo", menuName = "Estudo")]
public class Estudo : ScriptableObject
{
    public Sprite imagem;
    public int materia;
    public float preco;
    public string titulo;
    public string descricao;
}

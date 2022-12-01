using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controles : MonoBehaviour {

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text direita, esquerda, cima, baixo, atacar, aEspecial, dash, pause;

    private GameObject currentKey;

    private Color32 normal = new Color(1, 1, 1, 1);
    private Color32 selecionado = new Color32(87, 58, 1, 255);

    private void Start() {
        keys.Add("direita", KeyCode.X);
        keys.Add("esquerda", KeyCode.X);
        keys.Add("cima", KeyCode.X);
        keys.Add("baixo", KeyCode.X);
        keys.Add("atacar", KeyCode.X);
        keys.Add("ataqueEspecial", KeyCode.X);
        keys.Add("dash", KeyCode.X);
        keys.Add("pause", KeyCode.X);
        att();
    }

    void OnGUI() {
        if (currentKey != null) {
            Event e = Event.current;
            if (e.isKey || e.isMouse) {
                if (currentKey.name == "direita") {
                    MyInput.setDireita(e.keyCode);
                } else if (currentKey.name == "esquerda") {
                    MyInput.setEsquerda(e.keyCode);
                } else if (currentKey.name == "cima") {
                    MyInput.setCima(e.keyCode);
                } else if (currentKey.name == "baixo") {
                    MyInput.setBaixo(e.keyCode);
                } else if (currentKey.name == "atacar") {
                    MyInput.setAtacar(e.keyCode);
                } else if (currentKey.name == "ataqueEspecial") {
                    MyInput.setAtaqueEspecial(e.keyCode);
                } else if (currentKey.name == "dash") {
                    MyInput.setDash(e.keyCode);
                } else if (currentKey.name == "pause") {
                    MyInput.setPause(e.keyCode);
                }

                if (e.isKey) {
                    keys[currentKey.name] = e.keyCode;
                } else {
                    Debug.Log("A");
                    KeyCode mouseKeyCode = KeyCode.X;
                    switch (e.button) {
                        case 0:
                            mouseKeyCode = KeyCode.Mouse0;
                            break;
                        case 1:
                            mouseKeyCode = KeyCode.Mouse1;
                            break;
                        case 2:
                            mouseKeyCode = KeyCode.Mouse2;
                            break;
                    }
                    keys[currentKey.name] = mouseKeyCode;

                }
                currentKey.transform.GetChild(0).GetComponent<Text>().text = keys[currentKey.name].ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
        }
    }

    public void mudarInput(GameObject clicked) {
        if (currentKey != null) {
            currentKey.GetComponent<Image>().color = normal;
        }

        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selecionado;
    }


    public void restaurarPadroes() {
        MyInput.restaurarPadroes();
        att();
    }

    private void att() {
        keys["direita"] = MyInput.getDireita();
        keys["esquerda"] = MyInput.getEsquerda();
        keys["cima"] = MyInput.getCima();
        keys["baixo"] = MyInput.getBaixo();
        keys["atacar"] = MyInput.getAtacar();
        keys["ataqueEspecial"] = MyInput.getAtaqueEspecial();
        keys["dash"] = MyInput.getDash();
        keys["pause"] = MyInput.getPause();

        direita.text = keys["direita"].ToString();
        esquerda.text = keys["esquerda"].ToString();
        cima.text = keys["cima"].ToString();
        baixo.text = keys["baixo"].ToString();
        atacar.text = keys["atacar"].ToString();
        aEspecial.text = keys["ataqueEspecial"].ToString();
        dash.text = keys["dash"].ToString();
        pause.text = keys["pause"].ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyInput {
    private static KeyCode direita;
    private static KeyCode esquerda;
    private static KeyCode cima;
    private static KeyCode baixo;
    private static KeyCode atacar;
    private static KeyCode ataqueEspecial;
    private static KeyCode dash;
    private static KeyCode pause;

    static MyInput() {
        load();
    }

    public static void load() {
        if (PlayerPrefs.HasKey("direita")) {
            direita = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("direita"));
        } else {
            setDireita(KeyCode.D);
        }

        if (PlayerPrefs.HasKey("esquerda")) {
            esquerda = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("esquerda"));
        } else {
            setEsquerda(KeyCode.A);
        }

        if (PlayerPrefs.HasKey("cima")) {
            cima = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("cima"));
        } else {
            setCima(KeyCode.W);
        }

        if (PlayerPrefs.HasKey("baixo")) {
            baixo = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("baixo"));
        } else {
            setBaixo(KeyCode.S);
        }

        if (PlayerPrefs.HasKey("atacar")) {
            atacar = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("atacar"));
        } else {
            setAtacar(KeyCode.Mouse0);
        }

        if (PlayerPrefs.HasKey("ataqueEspecial")) {
            ataqueEspecial = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ataqueEspecial"));
        } else {
            setAtaqueEspecial(KeyCode.E);
        }

        if (PlayerPrefs.HasKey("dash")) {
            dash = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("dash"));
        } else {
            setDash(KeyCode.Space);
        }

        if (PlayerPrefs.HasKey("pause")) {
            pause = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pause"));
        } else {
            setPause(KeyCode.Escape);
        }
    }

    public static void restaurarPadroes() {
        setDireita(KeyCode.D);
        setEsquerda(KeyCode.A);
        setCima(KeyCode.W);
        setBaixo(KeyCode.S);
        setAtacar(KeyCode.Mouse0);
        setAtaqueEspecial(KeyCode.E);
        setDash(KeyCode.Space);
        setPause(KeyCode.Escape);
    }

    public static int getHorizontalAxis() {
        if (Input.GetKey(direita)) {
            return 1;
        } else if (Input.GetKey(esquerda)) {
            return -1;
        }
        return 0;
    }

    public static int getVerticalAxis() {
        if (Input.GetKey(cima)) {
            return 1;
        } else if (Input.GetKey(baixo)) {
            return -1;
        }
        return 0;
    }

    public static bool getAtacarAxis() {
        return Input.GetKey(atacar);
    }

    public static bool getAtaqueEspecialDownAxis() {
        return Input.GetKeyDown(ataqueEspecial);
    }

    public static bool getDashAxis() {
        return Input.GetKey(dash);
    }

    public static bool getPauseDownAxis() {
        return Input.GetKeyDown(pause);
    }

    //GETS e SETS

    public static void setDireita(KeyCode newDireita) {
        direita = newDireita;
        PlayerPrefs.SetString("direita", newDireita.ToString());
        PlayerPrefs.Save();
    }

    public static void setEsquerda(KeyCode newEsquerda) {
        esquerda = newEsquerda;
        PlayerPrefs.SetString("esquerda", newEsquerda.ToString());
        PlayerPrefs.Save();
    }

    public static void setCima(KeyCode newCima) {
        cima = newCima;
        PlayerPrefs.SetString("cima", newCima.ToString());
        PlayerPrefs.Save();
    }

    public static void setBaixo(KeyCode newBaixo) {
        baixo = newBaixo;
        PlayerPrefs.SetString("baixo", newBaixo.ToString());
        PlayerPrefs.Save();
    }


    public static void setAtacar(KeyCode newAtacar) {
        atacar = newAtacar;
        PlayerPrefs.SetString("atacar", newAtacar.ToString());
        PlayerPrefs.Save();
    }


    public static void setAtaqueEspecial(KeyCode newAtaqueEspecial) {
        ataqueEspecial = newAtaqueEspecial;
        PlayerPrefs.SetString("ataqueEspecial", newAtaqueEspecial.ToString());
        PlayerPrefs.Save();
    }


    public static void setDash(KeyCode newDash) {
        dash = newDash;
        PlayerPrefs.SetString("dash", newDash.ToString());
        PlayerPrefs.Save();
    }


    public static void setPause(KeyCode newPause) {
        pause = newPause;
        PlayerPrefs.SetString("pause", newPause.ToString());
        PlayerPrefs.Save();
    }

    public static KeyCode getDireita() {
        return direita;
    }

    public static KeyCode getEsquerda() {
        return esquerda;
    }
    public static KeyCode getCima() {
        return cima;
    }
    public static KeyCode getBaixo() {
        return baixo;
    }
    public static KeyCode getAtacar() {
        return atacar;
    }
    public static KeyCode getAtaqueEspecial() {
        return ataqueEspecial;
    }
    public static KeyCode getDash() {
        return dash;
    }
    public static KeyCode getPause() {
        return pause;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button btnContinuar;
    public GameObject confirmar;
    public AudioClip[] musicas;

    private bool temSave;

    void Start() {
        GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().setBackground(musicas[1]);
        GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playBg();

        temSave = PlayerPrefs.HasKey("save") && !PlayerPrefs.GetString("save").Equals("sem save");

        if (temSave) {
            btnContinuar.interactable = true;
        } else {
            btnContinuar.interactable = false;
        }
    }

    public void mostrarConfirmacao() {
        if (temSave) {
            confirmar.SetActive(true);
        } else {
            NovoJogo();
        }
    }

    public void NovoJogo()
    {
        PlayerStatus.morrer();
        PlayerStatus.excluirSave();
        SceneManager.LoadScene("Introducao", LoadSceneMode.Single);
    }

    public void Continuar() {
        PlayerStatus.morrer();
        PlayerStatus.load(musicas);
    }

    public void Sair()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PC : Interacao {
    public Trabalho[] trabalhos;
    public Estudo[] estudos;
    public GameObject abaInicial;
    public GameObject abaEstudo;
    public Text dinheiro;
    public Text dinheiroFLancer;
    public Canvas pcCanvas;
    public Conversa tutorial;
    public Button btnAvancarPagina;
    public Button btnVoltarPagina;
    public AudioSource musicaPC;
    public Text txtTrabalhosEmBreve;

    private int fLancerPagina;

    public void Start()
    {
        GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
        if (caixaDeSom != null && PlayerStatus.getProgresso() < 22)
        {
            caixaDeSom.GetComponent<MusicaDeFundo>().unPauseBg();
        }

        tutorial.enabled = false;
    }

    public void Update()
    {
    }

    public void programar(int trab) {
        Trabalho trabalho = trabalhos[trab + 4 * fLancerPagina];

        GameManager.setProximoTrabalho(trabalho);
        PlayerStatus.setUltimaCena(SceneManager.GetActiveScene().name);
        PlayerStatus.setUltimoPlayerX(GameObject.Find("Player").transform.position.x);
        SceneManager.LoadScene("Programar", LoadSceneMode.Single);
    }

    public void estudar(Estudo estudo) {
        if (!PlayerStatus.getInteligencia()[estudo.materia]) {
            if (PlayerStatus.getDinheiros() >= estudo.preco) {
                PlayerStatus.setDinheiros(PlayerStatus.getDinheiros() - estudo.preco);
                PlayerStatus.setInteligencia(estudo.materia, true);
                GameManager.showMessage("Você aprendeu " + estudo.titulo + "!", "Novo conhecimento");

                GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
                if (caixaDeSom != null)
                {
                    caixaDeSom.GetComponent<MusicaDeFundo>().playSound(51);
                }

                habilitarAbaEstudo(abaEstudo);
            } else {
                GameManager.showMessage("Você não tem dinheiro suficiente para estudar " + estudo.titulo + "...");
            }
        } else {
            GameManager.showMessage("Você já sabe " + estudo.titulo + "...");
        }
    }

    public void attAbaTrabalho() {
        dinheiroFLancer.text = "R$" + PlayerStatus.getDinheiros().ToString("0.00");
        Transform content = abaInicial.transform.GetChild(0).GetChild(0);

        for (int i = 0; i < 4; i++) {
            Trabalho trabalho = trabalhos[i + 4 * fLancerPagina];

            content.GetChild(i).GetChild(0).GetComponent<Image>().sprite = trabalho.cliente;
            content.GetChild(i).GetChild(1).GetComponent<Text>().text = trabalho.titulo;
            content.GetChild(i).GetChild(2).GetComponent<Text>().text = trabalho.descricao;
            content.GetChild(i).GetChild(3).GetComponent<Text>().text = "R$" + trabalho.salario.ToString("0.00");
        }

        txtTrabalhosEmBreve.text = (fLancerPagina + 1) + "/" + (PlayerStatus.getPaginasDesbloquadas() + 1);

        if (fLancerPagina == PlayerStatus.getPaginasDesbloquadas() && fLancerPagina != 2)
        {
            txtTrabalhosEmBreve.text += "\nMais trabalhos em breve.";
        }
    }

    public void attDinheiros()
    {
        dinheiroFLancer.text = "R$" + PlayerStatus.getDinheiros().ToString("0.00");
        dinheiro.text = "R$" + PlayerStatus.getDinheiros().ToString("0.00");
    }

    public void avancarPagina() {
        fLancerPagina++;
        attAbaTrabalho();
        if(fLancerPagina == PlayerStatus.getPaginasDesbloquadas()) {
            btnAvancarPagina.interactable = false;
        }
        if (fLancerPagina == 1) {
            btnVoltarPagina.interactable = true;
        }
    }

    public void voltarPagina() {
        fLancerPagina--;
        attAbaTrabalho();
        if (fLancerPagina == 0) {
            btnVoltarPagina.interactable = false;
        }
        if (fLancerPagina == PlayerStatus.getPaginasDesbloquadas() - 1) {
            btnAvancarPagina.interactable = true;
        }

    }

    public void habilitarAbaEstudo(GameObject content) {
        int bloquear = 0;
        if(PlayerStatus.getPaginasDesbloquadas() < 1)
        {
            bloquear = 7;
        } else if(PlayerStatus.getPaginasDesbloquadas() < 2)
        {
            bloquear = 3;
        }

        for(int i = content.transform.childCount - 1; i  >= content.transform.childCount - bloquear; i--)
        {
            Transform estudo = content.transform.GetChild(i);
            estudo.GetChild(0).GetComponent<Button>().interactable = false;
            estudo.GetChild(1).gameObject.SetActive(true);
        }

        for(int i = 0; i < content.transform.childCount; i++) {
            Transform estudo = content.transform.GetChild(i).GetChild(0);
            estudo.GetChild(0).GetComponent<Image>().sprite = estudos[i].imagem;
            estudo.GetChild(1).GetComponent<Text>().text = estudos[i].titulo;
            estudo.GetChild(2).GetComponent<Text>().text = estudos[i].descricao;
            estudo.GetChild(3).GetComponent<Text>().text = "R$" + estudos[i].preco.ToString("0.00");

            if (PlayerStatus.getInteligencia()[estudos[i].materia])
            {
                estudo.GetComponent<Button>().interactable = false;
            }
        }

        dinheiro.text = "R$" + PlayerStatus.getDinheiros().ToString("0.00");
    }

    public override void interagir() {
        pcCanvas.gameObject.SetActive(true);
        habilitarAbaEstudo(abaEstudo);
        attAbaTrabalho();
        abaInicial.SetActive(true);
        if (PlayerStatus.getPaginasDesbloquadas() > 0) {
            btnAvancarPagina.interactable = true;
        }
        if (!PlayerStatus.tutorialPC) {
            tutorial.enabled = true;
            PlayerStatus.tutorialPC = true;
        }

        GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
        if (caixaDeSom != null)
        {
            caixaDeSom.GetComponent<MusicaDeFundo>().pauseBg();
        }

        musicaPC.Play();
    }

    public void sair() {
        pcCanvas.gameObject.SetActive(false);

        GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
        if (caixaDeSom != null)
        {
            caixaDeSom.GetComponent<MusicaDeFundo>().playBg();
        }

        musicaPC.Pause();
    }
}

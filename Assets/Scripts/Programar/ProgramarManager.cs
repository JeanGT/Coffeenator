using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgramarManager : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject canvasFinal;
    public GameObject canvasUP;
    public GameObject canvasUP1;
    public GameObject canvasUP2;
    public GameObject branco;
    public GameObject botaoContinuar;
    public Text txtMsgFinal;
    public Text tituloMsgFinal;
    public float tempoBranco;

    public AudioClip somVitoria;
    public AudioClip somDerrota;
    public AudioClip somVitoriaHackear;
    public AudioClip musicaHackear;
    public AudioSource caixaProgramar;

    public static bool hackear;
    private bool acabou;
    public float cTempoBranco;

    private bool mostrouPainel;

    private bool entrou = false;
    private bool trocouMusica = false;
    private bool upou = false;

    public static MusicaDeFundo caixaDeSom;

    private bool isPaused = false;

    private void Awake() {
        hackear = PlayerStatus.getUltimaCena().Equals("Mapa");
    }

    void Start()
    {
        canvasFinal.SetActive(false);

        try
        {
            caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
        }
        catch (NullReferenceException)
        {

        }

        if (hackear)
        {
            canvasUP.SetActive(true);
            pause();
        }
        else
        {
            canvasUP.SetActive(false);
        }

        caixaDeSom.pauseBg();
        caixaProgramar.Play();

        if (GameManager.getProximoTrabalho().tutorial)
            Instantiate(tutorial, new Vector3(), Quaternion.identity);
    }

    private bool temBugsNaTela()
    {
        var objects = GameObject.FindGameObjectsWithTag("Bug");
        return objects.Length > 0;
    }

    void Update() {
        SpawnBug spawn = GameObject.Find("BugSpawn").GetComponent<SpawnBug>();
        ArrayList nextBugs = spawn.getNextBugs();
        ArrayList lastBugs = spawn.getLastBugs();

        if (nextBugs.Count == 0 && lastBugs.Count == 0 && !acabou && !temBugsNaTela())
        {
            if (hackear && !entrou)
            {
                if (spawn.avancarWave())
                {
                    acabou = true;
                    vencer();
                }
                entrou = true;
            }
            else
            {
                acabou = true;
                vencer();
            }
        } else if (entrou)
        {
            entrou = false;
        }

        if (spawn.getWave() == 1 && !upou)
        {
            canvasUP.SetActive(true);
            canvasUP2.SetActive(true);
            pause();
            upou = true;
        }
        else if (spawn.getWave() == 2) {
            if (!trocouMusica)
            {
                branco.SetActive(true);
                caixaProgramar.clip = musicaHackear;
                caixaProgramar.Play();

                trocouMusica = true;
            }

            if (cTempoBranco < tempoBranco)
            {
                branco.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1 - (cTempoBranco / tempoBranco));
                cTempoBranco += Time.deltaTime;
            }
        }
    }

    public void vencer() {
        if (!mostrouPainel) {
            mostrouPainel = true;
            canvasFinal.SetActive(true);
            float dinheiroAReceber = 0;

            tituloMsgFinal.color = new Color(0, 0.8f, 0);
            tituloMsgFinal.text = "Vitória";
            caixaProgramar.Stop();

            if (!hackear)
            {
                caixaDeSom.playSound(somVitoria);
                dinheiroAReceber = GameManager.getProximoTrabalho().salario;

                PlayerProgramar player = GameObject.Find("Player").GetComponent<PlayerProgramar>();

                if(player.HP == 3)
                {
                    dinheiroAReceber *= 1.2f;
                }

                txtMsgFinal.text = "Você ganhou R$" + dinheiroAReceber.ToString("0.00");

            } else
            {
                caixaDeSom.playSound(somVitoriaHackear);
                dinheiroAReceber = float.MinValue;
                txtMsgFinal.text = "Você descobriu a senha do wi-fi";
            }

            StartCoroutine(delayBotao());

            PlayerStatus.setDinheirosAReceber(dinheiroAReceber);
        }
    }

    public void perder() {
        if (!mostrouPainel) {
            mostrouPainel = true;
            canvasFinal.SetActive(true);
            caixaProgramar.clip = somDerrota;
            caixaProgramar.Play();
            tituloMsgFinal.color = new Color(0.8f, 0, 0);
            tituloMsgFinal.text = "Derrota...";

            GameObject[] bugs = GameObject.FindGameObjectsWithTag("Bug");
            foreach (GameObject bug in bugs)
            {
                bug.GetComponent<Enemy>().hurt(10000);
            }

            GameObject.Find("BugSpawn").GetComponent<SpawnBug>().clearNextBugs();

            if (hackear) {
                txtMsgFinal.text = "Você não conseguiu hackear o wi-fi";
            }

            StartCoroutine(delayBotao());
        }
    }

    public void terminar() {
        GameManager.setPlayerX(PlayerStatus.getUltimoPlayerX());
        GameManager.setPlayerOlhandoEsquerda(true);
        SceneManager.LoadScene(PlayerStatus.getUltimaCena(), LoadSceneMode.Single);
    }

    public bool getAcabou()
    {
        return acabou;
    }

    public void pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    IEnumerator delayBotao()
    {
        yield return new WaitForSeconds(1);

        botaoContinuar.SetActive(true);
    }
}

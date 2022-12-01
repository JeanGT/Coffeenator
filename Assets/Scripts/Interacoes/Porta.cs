using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Porta : Interacao {
    public string destino;
    public float playerX;
    public bool automatico;
    public bool trancadoANoite;
    public AudioClip musicaPadrao;
    public AudioClip musicaTensa;
    public bool playerOlhandoE;
    AsyncOperation asyncLoad;

    public bool notAsync;

    void Start () {
        StartObjeto();
        if (destino.Equals("Mapa") && !notAsync) {
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    // Update is called once per frame
    void Update () {
        UpdateObjeto();
    }

    IEnumerator LoadYourAsyncScene() {
        asyncLoad = SceneManager.LoadSceneAsync(destino);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    public override void interagir() {
        if (trancadoANoite && PlayerStatus.horario >= Sol.duracaoDia) {
            GameManager.showMessage("Estamos fechados, volte durante o dia.");
        } else {
            PlayerStatus.setUltimaCena(SceneManager.GetActiveScene().name);
            PlayerStatus.setUltimoPlayerX(GameObject.Find("Player").transform.position.x);
            GameManager.setPlayerX(playerX);
            GameManager.setPlayerOlhandoEsquerda(playerOlhandoE);
            if (destino.Equals("Mapa") && !notAsync) {
                asyncLoad.allowSceneActivation = true;
            } else {
                SceneManager.LoadScene(destino, LoadSceneMode.Single);
            }

            if (destino.Equals("Beco"))
            {
                if (PlayerStatus.getProgresso() > 6 && PlayerStatus.getProgresso() < 18)
                {
                    GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
                    if (caixaDeSom != null)
                    {
                        caixaDeSom.GetComponent<MusicaDeFundo>().pauseBg();
                    }
                }
            } else if (automatico)
            {
                if (PlayerStatus.getProgresso() > 6 && PlayerStatus.getProgresso() < 18)
                {
                    GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
                    if (caixaDeSom != null)
                    {
                        caixaDeSom.GetComponent<MusicaDeFundo>().unPauseBg();
                    }
                }
            } else
            {
                int som = Random.Range(65, 66);
                GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(som);
            }
        }
    }

    public bool isAutomatico() {
        return this.automatico;
    }
}

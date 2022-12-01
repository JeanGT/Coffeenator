using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fight : MonoBehaviour
{
    public float horizontalDistance;
    public float speed;
    public float minTime;
    public float tempoFadeout;
    public float velocidadeFlash;
    public Transform direita;
    public Transform esquerda;
    public GameObject vs;
    public GameObject flash;
    public AudioClip musicaFight;

    private float initialDireitaX;
    private float initialEsquerdaX;
    private float initialTempoFadout;
    AsyncOperation asyncLoad;

    void Start()
    {
        initialDireitaX = direita.position.x;
        initialEsquerdaX = esquerda.position.x;

        direita.position = new Vector3(direita.position.x + horizontalDistance, direita.position.y, direita.position.z);
        esquerda.position = new Vector3(esquerda.position.x - horizontalDistance, esquerda.position.y, esquerda.position.z);

        MusicaDeFundo caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
        if (caixaDeSom != null) {
            caixaDeSom.pauseBg();
            caixaDeSom.playSound(musicaFight);
        }

        initialTempoFadout = tempoFadeout;

        StartCoroutine(LoadYourAsyncScene());
    }

    void Update()
    {
        if(horizontalDistance > 0){
            float amount = Time.deltaTime * speed;

            direita.position = new Vector3(direita.position.x - amount, direita.position.y, direita.position.z);
            esquerda.position = new Vector3(esquerda.position.x + amount, esquerda.position.y, esquerda.position.z);
            horizontalDistance -= amount;
        } else
        {
            direita.position = new Vector3(initialDireitaX, direita.position.y, direita.position.z);
            esquerda.position = new Vector3(initialEsquerdaX, esquerda.position.y, esquerda.position.z);

            flash.SetActive(true);
            vs.SetActive(true);

            Image flashImage = flash.GetComponent<Image>();

            if (minTime < 0)
            {
                if (asyncLoad.progress > 0.89f)
                {
                    if (tempoFadeout < 0)
                    {
                        asyncLoad.allowSceneActivation = true;
                    }
                    else
                    {
                        tempoFadeout -= Time.deltaTime;
                        flashImage.color = new Color(0, 0, 0, (-1 / initialTempoFadout) * tempoFadeout + 1);
                    }
                }
            }
            else
            {
                if (flashImage.color.a > 0)
                    flashImage.color = new Color(1, 1, 1, flashImage.color.a - Time.deltaTime * velocidadeFlash);
            }
        }

        minTime -= Time.deltaTime;
    }

    IEnumerator LoadYourAsyncScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync("Combate");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

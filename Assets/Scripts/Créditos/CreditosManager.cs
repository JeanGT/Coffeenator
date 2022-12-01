using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditosManager : MonoBehaviour
{
    public GameObject[] creditos;
    public AudioClip musicaFinal;
    public GameObject creditosFinais;
    public Image preto;
    public float velocidadeCamera = 0.3f;
    public float tempoFadeout = 3;
    public float posicaoFinal = 66.66f;
    public float velocidadeCreditosFinais = 0.05f;
    public bool chegou = false;
    public float offSet = 5;

    public float duracaoMadrugada = 5; // Padrão: 5
    public float duracaoDia = 45; // Padrão: 200
    public float duracaoTardinha = 55; // Padrão: 215
    public Color luzDia; // Cor da luz durante o dia
    public Color luzTardinha; // Cor da luz durante a passagem do dia para a noite
    public Color luzNoite; // Cor da luz durante a noite

    private UnityEngine.Rendering.Universal.Light2D thisLight;

    private MusicaDeFundo caixaDeSom;
    private float initialTempoFadeout;
    private float alpha;
    private float tempo;
    private bool instanciou1;
    private bool instanciou2;
    private bool instanciou3;
    AsyncOperation asyncLoad;

    void Start()
    {
        caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
        caixaDeSom.setBackground(musicaFinal);
        caixaDeSom.playBg();
        caixaDeSom.background.loop = false;

        PlayerStatus.horario = 1;
        thisLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        initialTempoFadeout = tempoFadeout;
        StartCoroutine(LoadYourAsyncScene());
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStatus.horario += Time.deltaTime;

        if (transform.position.x < posicaoFinal)
        {
            this.transform.position = new Vector3(this.transform.position.x + Time.deltaTime * velocidadeCamera, this.transform.position.y, this.transform.position.z);
        }
        else
        {
            if (!chegou) {
                chegou = true;
                GameObject.Find("Player").SetActive(false);
                GameObject.Find("Nicole").SetActive(false);
                GameObject.Find("Alex").SetActive(false);
            }

            for(int i = 0; i < 3; i++)
                creditosFinais.transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, alpha);

            for (int i = 3; i < 8; i++)
                creditosFinais.transform.GetChild(i).GetComponent<Text>().color = new Color(1, 1, 1, alpha);

            alpha += Time.deltaTime * velocidadeCreditosFinais;
        }

        if(!caixaDeSom.background.isPlaying)
        {
            fadeOut();

            if (tempoFadeout < 0)
            {
                caixaDeSom.background.loop = true;
                asyncLoad.allowSceneActivation = true;
            }
        }

        if (PlayerStatus.horario < duracaoMadrugada)
        {
            float t = (duracaoMadrugada - PlayerStatus.horario) / duracaoMadrugada;
            thisLight.color = Color.Lerp(luzDia, luzNoite, t);
        }
        else if (PlayerStatus.horario < duracaoDia)
        {
            float t = (duracaoDia - PlayerStatus.horario) / (duracaoDia - duracaoMadrugada);
            thisLight.color = Color.Lerp(luzTardinha, luzDia, t);
        }
        else if (PlayerStatus.horario < duracaoTardinha)
        {
            float t = (duracaoTardinha - PlayerStatus.horario) / (duracaoTardinha - duracaoDia);
            thisLight.color = Color.Lerp(luzNoite, luzTardinha, t);
        }
        else
        {
            thisLight.color = luzNoite;
        }

        if (!instanciou1 && tempo > 10)
        {
            instanciarCredito(creditos[0]);
            instanciou1 = true;
        }
        if (!instanciou2 && tempo > 28)
        {
            instanciarCredito(creditos[1]);
            instanciou2 = true;
        }
        if (!instanciou3 && tempo > 46)
        {
            instanciarCredito(creditos[2]);
            instanciou3 = true;
        }

        tempo += Time.deltaTime;
    }

    private void fadeOut()
    {
        tempoFadeout -= Time.deltaTime;
        preto.color = new Color(0, 0, 0, preto.color.a + (-1 / initialTempoFadeout) * tempoFadeout + 1);
    }

    IEnumerator LoadYourAsyncScene()
    {
        yield return new WaitForSeconds(0.1f);

        asyncLoad = SceneManager.LoadSceneAsync("Menu");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void instanciarCredito(GameObject credito)
    {
        Instantiate(credito, new Vector3(transform.position.x + offSet, 0, 0), Quaternion.identity);
    }
}

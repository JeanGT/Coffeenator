using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombateManager : MonoBehaviour
{
    public AudioMixer efeitos;
    public GameObject tutorial;

    public GameObject[] inimigos;
    public Sprite[] backgrounds;
    public AudioClip[] musicas;
    public AudioClip musicaDerrota;
    public AudioClip musicaDerrotaBoss;
    public SpriteRenderer fundo;
    public AudioSource musicaBG;
    public float tempoAteIniciar;
    public Text txtCountdown;
    public static float tempoAteAcabar = -1f;
    public static bool ganhou;
    public GameObject painelFinal;
    public Text tituloFinal;
    public Text textFinal;
    public Text txtNomeBoss;
    public static int final;
    public static string msgFinal;
    public float velocidadeVersus;
    public float horizontalVersus;
    public Image preto;
    public float velocidadeFadeIn = 0.5f;
    public int count = 4;

    private GameObject inimigo;
    public float precoMorrer = 5;
    private bool iniciouMusicaPerdeu = false;

    private float cTempoAteIniciar;
    private bool isBoss = false;
    private bool mostrouAvisoMendigo = false;

    void Start() {
        fundo.sprite = backgrounds[PlayerStatus.getProximaBatalha()];
        musicaBG.clip = musicas[PlayerStatus.getProximaBatalha()];

        musicaBG.Play();

        GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
        if(caixaDeSom != null) {
            caixaDeSom.GetComponent<MusicaDeFundo>().pauseBg();
        }

        tempoAteAcabar = -1f;
        ganhou = false;
        final = -2;
        msgFinal = "";

        inimigo = Instantiate(inimigos[PlayerStatus.getProximaBatalha()], transform.position, Quaternion.identity);
        painelFinal.SetActive(false);
        Time.timeScale = 0.001f;

        txtNomeBoss.text = inimigo.GetComponent<Inimigo>().nome;

        if (PlayerStatus.getProximaBatalha() == 10)
            Instantiate(tutorial, new Vector3(), Quaternion.identity);

        isBoss = inimigo.GetComponent<Inimigo>().isBoss;
    }

    private void Update() {
        if (count >= 0)
        {
            preto.color = new Color(0, 0, 0, 1 - (cTempoAteIniciar + (4 - count) * (tempoAteIniciar/4)) * velocidadeFadeIn);

            cTempoAteIniciar += Time.deltaTime * 1000;
            if (cTempoAteIniciar > tempoAteIniciar / 4)
            {
                cTempoAteIniciar = 0;
                count--;
                if (count < 0)
                {
                    Time.timeScale = 1f;
                    Destroy(txtCountdown.gameObject);
                }
                else if (count == 0)
                {
                    txtCountdown.text = "Lutem!";
                    GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(49);
                }
                else
                {
                    txtCountdown.text = (count + 1).ToString();
                }
            }
        }
        else if (tempoAteAcabar != -1f)
            {
                tempoAteAcabar -= Time.deltaTime;
                if (tempoAteAcabar < 0)
                {
                    painelFinal.SetActive(true);

                    StartCoroutine(delayBotao());
                }
                if (ganhou)
                {
                    tituloFinal.text = "Vitória!";

                    textFinal.text = msgFinal;
                }
                else
                {
                    switch (final)
                    {
                        case -1:
                            tituloFinal.text = "Derrota...";
                            tituloFinal.color = new Color(0.8f, 0, 0);
                            textFinal.text = "O Orochi te deu uma surra";

                            if (!iniciouMusicaPerdeu) {
                                musicaBG.clip = musicaDerrota;
                                musicaBG.Play();

                                iniciouMusicaPerdeu = true;
                            }

                            break;
                        case 0:
                            tituloFinal.text = "Derrota...";
                            tituloFinal.color = new Color(0.8f, 0, 0);
                            textFinal.text = "Você foi levado até o hospital";

                            if (!iniciouMusicaPerdeu)
                            {
                                musicaBG.clip = musicaDerrota;
                                musicaBG.Play();

                                iniciouMusicaPerdeu = true;
                            }

                        break;
                        case 1:
                            if (!mostrouAvisoMendigo)
                            {
                                if (PlayerStatus.getAvisoMendigo() == 1)
                                {
                                    tituloFinal.text = "Derrota...";
                                    tituloFinal.color = new Color(0.8f, 0, 0);
                                    textFinal.text = "Você foi levado até o hospital";

                                    if (!iniciouMusicaPerdeu)
                                    {
                                        musicaBG.clip = musicaDerrota;
                                        musicaBG.Play();

                                        iniciouMusicaPerdeu = true;
                                    }

                                }
                                else
                                {
                                    tituloFinal.text = "Você morreu...";
                                    tituloFinal.color = new Color(0.8f, 0, 0);
                                    textFinal.text = "Lembre-se do porquê você precisa dessa chave, sua família te espera...\nVocê voltará para o último save";

                                    if (!iniciouMusicaPerdeu)
                                    {
                                        musicaBG.clip = musicaDerrotaBoss;
                                        musicaBG.Play();

                                        iniciouMusicaPerdeu = true;
                                    }

                                }

                                mostrouAvisoMendigo = true;
                            }
                            break;
                        case 2:
                            tituloFinal.text = "Você morreu...";
                            tituloFinal.color = new Color(0.8f, 0, 0);
                            textFinal.text = "Lembre-se do porquê você precisa derrotá-lo, sua família te espera...\nVocê voltará para o último save";

                            if (!iniciouMusicaPerdeu)
                            {
                                musicaBG.clip = musicaDerrotaBoss;
                                musicaBG.Play();

                                iniciouMusicaPerdeu = true;
                            }

                        break;
                        case 3:
                            tituloFinal.text = "Você morreu...";
                            tituloFinal.color = new Color(0.8f, 0, 0);
                            textFinal.text = "A cidade precisa de você, detetive! Não desista!\nVocê voltará para o último save";

                            if (!iniciouMusicaPerdeu)
                            {
                                musicaBG.clip = musicaDerrotaBoss;
                                musicaBG.Play();

                                iniciouMusicaPerdeu = true;
                            }

                        break;
                        case 4:
                            tituloFinal.text = "Derrota...";
                            tituloFinal.color = new Color(0.8f, 0, 0);
                            textFinal.text = "Você foi levado até o hospital";

                            if (!iniciouMusicaPerdeu)
                            {
                                musicaBG.clip = musicaDerrota;
                                musicaBG.Play();

                                iniciouMusicaPerdeu = true;
                            }

                        break;
                        default:
                            tituloFinal.text = "Derrota...";
                            tituloFinal.color = new Color(0.8f, 0, 0);

                            if (!iniciouMusicaPerdeu)
                            {
                                musicaBG.clip = musicaDerrota;
                                musicaBG.Play();

                                iniciouMusicaPerdeu = true;
                            }

                        break;
                    }
                }
            }
        }

    public void terminar()
    {
        GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playBg();

        switch (final) {
            case -1:
                PlayerStatus.horario = Sol.duracaoTardinha;
                GameManager.setPlayerX(PlayerStatus.getUltimoPlayerX());
                GameManager.setPlayerOlhandoEsquerda(false);
                SceneManager.LoadScene("Dojo", LoadSceneMode.Single);
                break;
            case 0:
                if (ganhou) {
                    GameManager.isLevelUp = true;
                    GameManager.setPlayerX(PlayerStatus.getUltimoPlayerX());
                    GameManager.setPlayerOlhandoEsquerda(false);
                    SceneManager.LoadScene("Dojo", LoadSceneMode.Single);
                } else {
                    GameManager.setPlayerOlhandoEsquerda(true);
                    SceneManager.LoadScene("Hospital", LoadSceneMode.Single);
                }
                break;
            case 1:
                if (ganhou) {
                    if (PlayerStatus.getAvisoMendigo() == 1)
                    {
                        PlayerStatus.setAvisoMendigo(-1);
                        GameManager.setPlayerOlhandoEsquerda(true);
                        SceneManager.LoadScene("Beco", LoadSceneMode.Single);
                    }
                    else
                    {
                        GameManager.setPlayerX(PlayerStatus.getUltimoPlayerX());
                        GameManager.setPlayerOlhandoEsquerda(true);
                        SceneManager.LoadScene("Beco", LoadSceneMode.Single);
                        ChaveMendigo.derrotou = true;
                    }
                } else {
                    if (PlayerStatus.getAvisoMendigo() == 1)
                    {
                        PlayerStatus.setAvisoMendigo(0);
                        GameManager.setPlayerOlhandoEsquerda(true);
                        SceneManager.LoadScene("Hospital", LoadSceneMode.Single);
                    } else
                    {
                        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
                    }
                }
                break;
            case 2:
                if (ganhou) {
                    GameManager.setPlayerOlhandoEsquerda(false);
                    SceneManager.LoadScene("Armazem", LoadSceneMode.Single);
                } else {
                    SceneManager.LoadScene("Menu", LoadSceneMode.Single);
                }
                break;
            case 3:
                if (ganhou) {
                    GameManager.setPlayerOlhandoEsquerda(false);
                    SceneManager.LoadScene("DerrotaOrochi", LoadSceneMode.Single);
                } else {
                    SceneManager.LoadScene("Menu", LoadSceneMode.Single);
                }
                break;
            case 4:
                if (ganhou) {
                    GameManager.setPlayerOlhandoEsquerda(false);
                    GameManager.isLevelUp = true;
                    GameManager.isNovaPagina = true;
                    GameManager.setPlayerX(PlayerStatus.getUltimoPlayerX());
                    SceneManager.LoadScene("Dojo", LoadSceneMode.Single);
                } else {
                    SceneManager.LoadScene("Hospital", LoadSceneMode.Single);
                }
                break;
        }
    }

    IEnumerator delayBotao()
    {
        GameObject botaoConfirmar = painelFinal.transform.GetChild(2).gameObject;

        yield return new WaitForSeconds(1);

        if (isBoss)
            yield return new WaitForSeconds(2);

        botaoConfirmar.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ArmazemManager : MonoBehaviour
{
    public GameObject[] luzes;
    public float tempoTudoEscuro;
    public GameObject caio;
    public GameObject caioNocauteado;
    public GameObject policial1;
    public GameObject policial2;
    public GameObject portaAberta;
    public GameObject canvas;
    public GameObject barraCutscene;
    public CameraController cam;
    public Player player;
    public FadeIn fadeIn;
    public Blink blink;
    public float velocidadeFadeIn;
    public float tempoPolicialParado;
    public float xPolicial1;
    public float velocidadePolicial;
    public float distanciaPolicial;
    public float velocidadeBarraC;
    public float tamanhoBarraC;
    public float aliadoOffset;

    private float targetYBarraC;

    private float cTempoPolicialParado;

    private bool terminouFadeIn;
    private float cTempoTudoEscuro;
    private bool primeiroFrame = true;
    private bool policiaisEntraram = false;
    private bool caioApareceu = false;

    void Start()
    {
        setIntensidade(0);
        player.setFreeze(true);
        policial1.SetActive(false);
        policial2.SetActive(false);
        caioNocauteado.SetActive(false);
        canvas.SetActive(false);

        GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
        if (caixaDeSom != null)
        {
            caixaDeSom.GetComponent<MusicaDeFundo>().pauseBg();
        }

        GameObject nicole = GameObject.Find("Nicole");
        GameObject alex = GameObject.Find("Alex");

        nicole.GetComponent<Aliado>().enabled = false;
        alex.GetComponent<Aliado>().enabled = false;

        nicole.transform.position = new Vector3(nicole.transform.position.x - aliadoOffset, nicole.transform.position.y, nicole.transform.position.z);
        alex.transform.position = new Vector3(alex.transform.position.x - aliadoOffset * 2, -0.175f, alex.transform.position.z);

        targetYBarraC = barraCutscene.transform.position.y - tamanhoBarraC;
    }

    // Update is called once per frame
    void Update()
    {
        if (!terminouFadeIn) {
            float intesidade = luzes[0].GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity;
            setIntensidade(intesidade + velocidadeFadeIn * Time.deltaTime);

            if(intesidade >= 1) {
                setIntensidade(1);
                terminouFadeIn = true;
                player.setFreeze(false);
            }
        }

        if (luzes[0].GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity > 0.5) {
            abaixarBarra();
        }

        if (PlayerStatus.getProgresso() == 20) {
            if (primeiroFrame) {
                GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
                if (caixaDeSom != null)
                {
                    caixaDeSom.GetComponent<MusicaDeFundo>().playSound(62);
                }

                primeiroFrame = false;
                attLuzes(false);
            }
            cTempoTudoEscuro += Time.deltaTime;

            if (cTempoTudoEscuro > tempoTudoEscuro) {
                if (!caioApareceu)
                {
                    GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
                    if (caixaDeSom != null)
                    {
                        caixaDeSom.GetComponent<MusicaDeFundo>().playSound(47);
                        caioApareceu = true;
                    }
                }

                attLuzes(true);
                caio.SetActive(true);
            }
        }

        if (PlayerStatus.getProgresso() == 22) {
            caioNocauteado.SetActive(true);
            caio.SetActive(false);
        }

        if (PlayerStatus.getProgresso() == 200) {
            if (!policiaisEntraram) {
                portaAberta.SetActive(true);
                policial1.SetActive(true);
                policial2.SetActive(true);
                player.setFreeze(true);
                policiaisEntraram = true;

                GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playSound(65);

                cam.player = policial1.transform;
                LookAt.setTargetName("Policial1");
            }
        }

        if(PlayerStatus.getProgresso() == 202) {
            StartCoroutine(policiaisOlhandoArmazem());

            PlayerStatus.setProgresso(203);
        }

        if(PlayerStatus.getProgresso() == 204) {
            policial1.transform.localScale = new Vector3(1, 1, 1);
        }

        if (PlayerStatus.getProgresso() == 205) {
            caioNocauteado.GetComponent<Animator>().SetBool("algemado", true);
            StartCoroutine(policialLevandoCaio());

            PlayerStatus.setProgresso(206);
        }


        if (PlayerStatus.getProgresso() == 207) {
            player.setFreeze(true);
            cam.player = caioNocauteado.transform;
            LookAt.setTargetName("CaioNocauteado");
        }

        if (PlayerStatus.getProgresso() == 209) {
            cam.player = GameObject.Find("Player").transform;
            LookAt.setTargetName("Player");
            cTempoTudoEscuro = 0;
        }

        if (PlayerStatus.getProgresso() == 210) {
            blink.blink();

            cTempoTudoEscuro += Time.deltaTime;
            if (cTempoTudoEscuro > 1) {
                PlayerStatus.setProgresso(211);
                cTempoTudoEscuro = 0;
                blink.resetBlink();
            }
        }

        if (PlayerStatus.getProgresso() == 212) {
            blink.blink();

            cTempoTudoEscuro += Time.deltaTime;
            if (cTempoTudoEscuro > 1) {
                PlayerStatus.setProgresso(213);
                cTempoTudoEscuro = 0;
            }
        }

        if (PlayerStatus.getProgresso() == 213) {
            fadeIn.fadeIn();

            cTempoTudoEscuro += Time.deltaTime;
            if (cTempoTudoEscuro > 3) {
                cTempoTudoEscuro = 0;
            }
        }

        if (PlayerStatus.getProgresso() == 214) {
            cTempoTudoEscuro += Time.deltaTime;
            if (cTempoTudoEscuro > 5) {
                GameManager.setPlayerX(0.34f);
                PlayerStatus.desativarNicole();
                PlayerStatus.desativarAlex();
                PlayerStatus.setProgresso(22);
                GameManager.setPlayerOlhandoEsquerda(true);
                SceneManager.LoadScene("Quarto", LoadSceneMode.Single);
            }
        }
    }

    void FixedUpdate()
    {
        if (PlayerStatus.getProgresso() == 200) {
            if (cTempoPolicialParado < tempoPolicialParado) {
                cTempoPolicialParado += Time.fixedDeltaTime;
            } else {
                if (policial1.transform.position.x >= xPolicial1) {
                    policial1.GetComponent<Animator>().SetBool("movendo", true);
                    policial1.transform.position -= new Vector3(Time.fixedDeltaTime * velocidadePolicial, 0, 0);
                    if (policial2.transform.position.x - policial1.transform.position.x > distanciaPolicial) {
                        policial2.transform.position -= new Vector3(Time.fixedDeltaTime * velocidadePolicial, 0, 0);
                        policial2.GetComponent<Animator>().SetBool("movendo", true);
                    }

                    caioNocauteado.GetComponent<Animator>().SetBool("virado", true);
                    caioNocauteado.transform.position = new Vector3(caioNocauteado.transform.position.x, 0.105f, caioNocauteado.transform.position.z);
                } else {
                    policial1.GetComponent<Animator>().SetBool("movendo", false);
                    policial2.GetComponent<Animator>().SetBool("movendo", false);
                    PlayerStatus.setProgresso(201);
                    player.setFreeze(false);
                }
            }
        }

        if (PlayerStatus.getProgresso() == 207) {
            if (policial1.transform.position.x <= GameObject.Find("Player").GetComponent<Player>().maxX / 2) {
                policial1.transform.position += new Vector3(Time.fixedDeltaTime * velocidadePolicial * 0.8f, 0, 0);
                policial1.GetComponent<Animator>().SetBool("movendo", true);
                if (policial1.transform.position.x - caioNocauteado.transform.position.x > distanciaPolicial) {
                    caioNocauteado.transform.position += new Vector3(Time.fixedDeltaTime * velocidadePolicial * 0.8f, 0, 0);
                    caioNocauteado.GetComponent<Animator>().SetBool("andando", true);
                    caioNocauteado.transform.localScale = new Vector3(1, 1, 1);
                }

                if (policial1.transform.position.x - policial2.transform.position.x > distanciaPolicial * 3) {
                    policial2.transform.position += new Vector3(Time.fixedDeltaTime * velocidadePolicial * 0.8f, 0, 0);
                    policial2.GetComponent<Animator>().SetBool("movendo", true);
                }
            } else {
                policial2.GetComponent<Animator>().SetBool("movendo", false);
                caioNocauteado.GetComponent<Animator>().SetBool("andando", false);
                caioNocauteado.transform.localScale = new Vector3(-1, 1, 1);
                policial1.GetComponent<Animator>().SetBool("movendo", false);
                PlayerStatus.setProgresso(208);
                player.setFreeze(false);
            }
        }

        if (PlayerStatus.getProgresso() >= 209) {
            if (policial2.transform.position.x <= GameObject.Find("Player").GetComponent<Player>().maxX+1) {
                policial1.transform.position += new Vector3(Time.fixedDeltaTime * velocidadePolicial * 0.8f, 0, 0);
                policial1.GetComponent<Animator>().SetBool("movendo", true);
                caioNocauteado.transform.position += new Vector3(Time.fixedDeltaTime * velocidadePolicial * 0.8f, 0, 0);
                caioNocauteado.GetComponent<Animator>().SetBool("andando", true);
                caioNocauteado.transform.localScale = new Vector3(1, 1, 1);
                policial2.transform.position += new Vector3(Time.fixedDeltaTime * velocidadePolicial * 0.8f, 0, 0);
                policial2.GetComponent<Animator>().SetBool("movendo", true);
            }
        }
    }

    IEnumerator policiaisOlhandoArmazem() {
        player.setFreeze(true);

        policial1.transform.localScale = new Vector3 (-1, 1, 1);
        yield return new WaitForSeconds(1);

        policial2.transform.localScale = new Vector3(-1, 1, 1);
        yield return new WaitForSeconds(1);

        policial1.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(1);

        policial2.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(1);

        policial1.transform.localScale = new Vector3(-1, 1, 1);
        yield return new WaitForSeconds(1);

        player.setFreeze(false);
    }

    IEnumerator policialLevandoCaio() {
        player.setFreeze(true);

        yield return new WaitForSeconds(1.5f);
        policial1.transform.localScale = new Vector3(-1, 1, 1);
        yield return new WaitForSeconds(1);
        policial2.transform.localScale = new Vector3(-1, 1, 1);

        player.setFreeze(false);
    }

    private void attLuzes(bool estado) {
        foreach(GameObject l in luzes) {
            l.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = estado;
        }
    }

    private void setIntensidade(float intensidade) {
        foreach (GameObject l in luzes) {
            l.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = intensidade;
        }

    }

    private void abaixarBarra() {
        if(barraCutscene.transform.position.y > targetYBarraC) {
            barraCutscene.transform.position = new Vector3(barraCutscene.transform.position.x, barraCutscene.transform.position.y - Time.deltaTime * velocidadeBarraC, barraCutscene.transform.position.z);
        } else {
            barraCutscene.transform.position = new Vector3(barraCutscene.transform.position.x, targetYBarraC, barraCutscene.transform.position.z);
        }
    }
}

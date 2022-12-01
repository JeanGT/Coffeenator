using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CutsceneDojo : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D[] tochas; //Inner radius: 0.05f, Outer radius: 1.5f, Falloff: 0.8f, Intensity: 1.8f
    public UnityEngine.Rendering.Universal.Light2D[] highlights;
    public GameObject player;
    public GameObject canvas;
    public GameObject orochiNormal;
    public GameObject orochiSentado;
    public GameObject barrisSemOrochi;
    public GameObject orochiTransformado;
    public GameObject cameraHolder; //x: 0.5f, y: 0.25f
    public Image brilho;
    public CameraController cam;
    public AudioClip musicaCutscene;
    public float playerX;
    public float velocidadePlayer;
    public float velocidadeFadeOut;

    private bool executou = false;
    private bool transformou = false;
    private bool transformou2 = false;
    private bool carregado = true;
    private bool salvou = false;

    void Start()
    {
        canvas.SetActive(false);
        orochiNormal.SetActive(false);
        orochiSentado.SetActive(true);
        barrisSemOrochi.SetActive(false);
        orochiTransformado.SetActive(false);
        brilho.enabled = false;

        GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
        if (caixaDeSom != null)
        {
            caixaDeSom.GetComponent<MusicaDeFundo>().setBackground(musicaCutscene);
            caixaDeSom.GetComponent<MusicaDeFundo>().playBg();
        }
    }

    void Update()
    {
        if (PlayerStatus.getProgresso() == 302)
        {
            StartCoroutine(acenderTochasC());
            carregado = false;
        }

        if(PlayerStatus.getProgresso() == 304)
        {
            if (!salvou)
            {
                PlayerStatus.setUltimoPlayerX(playerX);
                PlayerStatus.save();
                salvou = true;
            }

            if (carregado)
            {
                cam.player = cameraHolder.transform;
                acenderTochas(0.05f, 1.5f, 1.8f);
            }

            orochiNormal.SetActive(true);
            orochiSentado.SetActive(false);
            barrisSemOrochi.SetActive(true);

            StartCoroutine(transformarOrochi());
        }

        if(PlayerStatus.getProgresso() == 305)
        {
            StartCoroutine(fadeOutBrilho());
        }
    }

    private void FixedUpdate()
    {
        if (PlayerStatus.getProgresso() == 300)
        {
            if (player.transform.position.x < playerX)
            {
                player.transform.position += new Vector3(Time.fixedDeltaTime * velocidadePlayer * 0.5f, 0, 0);
                player.transform.GetChild(1).GetComponent<Animator>().SetBool("movendo", true);
            }
            else
            {
                player.transform.position = new Vector3(playerX, -0.027f, 0);
                player.transform.GetChild(1).GetComponent<Animator>().SetBool("movendo", false);
                cam.player = cameraHolder.transform;
                PlayerStatus.setProgresso(301);
            }
        }
    }

    private void acenderTochas(float innerRadius, float outerRadius, float intensity)
    {
        for (int i = 0; i <= 6; i++)
        {
            tochas[i].pointLightInnerRadius = innerRadius;
            tochas[i].pointLightOuterRadius = outerRadius;
            tochas[i].intensity = intensity;

            tochas[i].gameObject.GetComponent<FireFlick>().enabled = true;
        }

        for (int j = 0; j < highlights.Length; j++)
        {
            highlights[j].intensity = 0;
        }
    }

    IEnumerator acenderTochasC()
    {
        if (!executou) {
            executou = true;

            yield return new WaitForSeconds(1f);

            acenderTochas(0.05f, 1.5f, 1.8f);

            yield return new WaitForSeconds(3f);

            PlayerStatus.setProgresso(303);
        }
    }

    IEnumerator transformarOrochi()
    {
        orochiNormal.GetComponent<Animator>().SetBool("caneca", true);
        orochiNormal.transform.localScale = new Vector3(1, 1, 1);
        GameObject caixaDeSom = GameObject.Find("MusicaPlayer");

        yield return new WaitForSeconds(1.5f);
        if (caixaDeSom != null && !transformou)
        {
            caixaDeSom.GetComponent<MusicaDeFundo>().playSound(64);
            transformou = true;
        }

        orochiNormal.GetComponent<Animator>().SetBool("transformando", true);

        if (orochiNormal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("OrochiTransformando"))
        {
            yield return new WaitForSeconds(1);

            if (caixaDeSom != null && !transformou2)
            {
                caixaDeSom.GetComponent<MusicaDeFundo>().pauseBg();
                caixaDeSom.GetComponent<MusicaDeFundo>().playSound(5);
                transformou2 = true;
            }

            brilho.enabled = true;
            orochiNormal.GetComponent<Animator>().SetBool("transformando", false);
            orochiNormal.transform.position = new Vector3(orochiNormal.transform.position.x, 0.079f, orochiNormal.transform.position.z); ;
            PlayerStatus.setProgresso(305);
        }
    }

    IEnumerator fadeOutBrilho()
    {
        if (PlayerStatus.getProgresso() < 306)
        {
            yield return new WaitForSeconds(1.5f);

            if (brilho.color.a > 0)
            {
                brilho.color = new Color(brilho.color.r, brilho.color.g, brilho.color.b, brilho.color.a - Time.deltaTime * velocidadeFadeOut);
            }
            else
            {
                brilho.color = new Color(brilho.color.r, brilho.color.g, brilho.color.b, 0);
                PlayerStatus.setProgresso(306);
            }
        }
    }
}

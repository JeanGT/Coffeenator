using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalManager : MonoBehaviour {
    public Player player;
    public CameraController cam;
    public AudioClip musicaFinal;

    public DiminuirIntensidade l1;
    public DiminuirIntensidade l2;
    public DiminuirIntensidade l3;

    void Start() {
        player.setFreeze(true);
        GameManager.getFundoPreto().SetActive(true);
        GameManager.getFundoPreto().GetComponent<FadeOut>().fadeOut();
        LookAt.setTargetName("Orochi");
    }

    // Update is called once per frame
    void Update() {
        if (!GameManager.getFundoPreto().activeInHierarchy) {
            player.setFreeze(false);
        }

        if (PlayerStatus.getProgresso() == 30) {
            cam.player = GameObject.Find("Epaminondas").transform;
        } else if (PlayerStatus.getProgresso() == 31) {
            cam.player = GameObject.Find("Player").transform;
            cam.quantidadeSuavizacao = 0.5f;
            LookAt.setTargetName("Player");
        } else if (PlayerStatus.getProgresso() == 32) {
            l1.comecou = true;
            l2.comecou = true;
            l3.comecou = true;

            if (l2.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity <= 0) {
                l1.comecou = false;
                l2.comecou = false;
                l3.comecou = false;
                PlayerStatus.setProgresso(33);
                GameManager.getFundoPreto().SetActive(true);
                GameManager.getFundoPreto().GetComponent<Image>().color = new Color(0, 0, 0, 1);

                GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
                if (caixaDeSom != null)
                {
                    caixaDeSom.GetComponent<MusicaDeFundo>().setBackground(musicaFinal);
                    caixaDeSom.GetComponent<MusicaDeFundo>().playBg();
                }
            }
        } else if (PlayerStatus.getProgresso() == 33 && !GameManager.getFundoPreto().activeInHierarchy) {
            SceneManager.LoadScene("Introducao", LoadSceneMode.Single);
        }


    }
}

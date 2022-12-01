using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConversaPrologo : Conversa
{
    public ConversaScriptable conversaFinal;
    public GameObject painelFinalDeVerdade;

    // Start is called before the first frame update
    public Animator ani;
    public GameObject painelFinal;
    public InputField txt;
    public GameObject monologoFundo;
    public Button botaoConfirmar;
    public AudioClip musicaBG;

    public float tempoMinimoPularMsg;
    private float cTempoMinimoPularMsg;

    protected override void Start() {
        GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().setBackground(null);
        base.Start();

        if(PlayerStatus.getProgresso() == 33) {
            conversa = conversaFinal;
        }

        interagir();
        painelFinal.SetActive(false);
        monologoFundo.SetActive(false);
        painelFinalDeVerdade.SetActive(false);
    }

    protected override void Update() {
        if(PlayerStatus.getProgresso() == 34) {
            painelFinalDeVerdade.SetActive(true);
            painelFinalDeVerdade.GetComponent<FadeIn>().fadeIn();
            if (painelFinalDeVerdade.GetComponent<Image>().color.a > 0.9f) {
                painelFinalDeVerdade.transform.GetChild(0).GetComponent<FadeIn>().fadeIn();
                cTempoMinimoPularMsg += Time.deltaTime;
                if(cTempoMinimoPularMsg > tempoMinimoPularMsg) {
                    if (Input.GetMouseButton(0)) {
                        SceneManager.LoadScene("Créditos", LoadSceneMode.Single);
                    }
                }
            }
        } else {
            base.Update();
        }
    }

    protected override void playSound() {
        if (conversationBox.activeInHierarchy) {
            base.playSound();
        }
    }

    public override void interagir() {
        conversationBox = GameObject.Find("conversationBox");
        base.interagir();
    }

    protected override void onDialogueChange() {
        ani.SetInteger("progresso", cDialogue);
    }

    protected override void hideDialogueBox() {
        base.hideDialogueBox();
        if (monologoFundo.activeInHierarchy) {
            MusicaDeFundo caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
            if (caixaDeSom != null)
                caixaDeSom.setBackground(musicaBG);

            caixaDeSom.playBg();

            GameManager.setPlayerOlhandoEsquerda(false);
            SceneManager.LoadScene("Casa", LoadSceneMode.Single);
        } else if (PlayerStatus.getProgresso() != 34) {
            painelFinal.SetActive(true);
        }
    }

    public void confirmarNome() {
        PlayerStatus.nome = txt.text;
        painelFinal.SetActive(false);
        monologoFundo.SetActive(true);
        conversationBox.SetActive(true);
        cTempoMinimoAtePular = 0;
        cDialogue = 0;

        interagir();
    }

    public void attBotao() {
        botaoConfirmar.interactable = txt.text.Length > 2;
    }

}

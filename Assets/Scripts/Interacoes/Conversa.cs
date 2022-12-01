using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Conversa : Interacao {
    public ConversaScriptable conversa;

    public float DELAY_VIRGULA = 0.2f;
    public float DELAY_PONTO = 0.4f;
    public bool fundoPreto;
    public AudioClip proximaMusicaDeFundo;
    public VideoClip cutscene;
    public bool chaveBob;
    public bool chaveMendigo;
    public int objetivo = -1;
    public int avancarObjetivo = -1;
    public int avancarObjetivo2 = -1;
    public string destino;
    public int proximaBatalha = -1;
    public int nivelMinimoDash = -1;
    public int nivelMaximoDash = -1;
    public int nivelMinimoProjetil = -1;
    public int nivelMaximoProjetil = -1;
    public int nivelMinimoEspecial = -1;
    public int nivelMaximoEspecial = -1;
    public bool desabilitarAposUsar;
    public Sprite spriteEnquantoConversa;
    public string parametroAnimacao;
    public bool ativaNicole;
    public bool ativaAlex;
    public bool parar;
    public bool salvar;
    public float playerX = -1;
    public bool precisaAvisoMendigo;
    public bool superUmaVez;
    public bool playerOlhandoEsquerda;

    private float tempoMinimoAtePular = 0.5f; //0.5f padrao

    protected float cTempoMinimoAtePular;

    private const float soundSpeed = 6f;

    private float cSoundSpeed;
    private Animator anim;
    private SpriteRenderer sp;
    private Sprite spriteAntesDeConversar;
    protected GameObject conversationBox;
    private Text conversationText;
    private Text nomesText;
    private Text indiceText;
    private bool conversando = false;
    protected int cDialogue = 0;
    private float progresso = 1;
    private float cDelay = 0;
    private PauseMenu pauseMenu;
    private GameManager gameManager;
    private bool pulou;

    // Use this for initialization
    protected virtual void Start () {
        if (GameObject.Find("GameManeger") != null) {
            gameManager = GameObject.Find("GameManeger").GetComponent<GameManager>();
        }

        conversa = conversa.att();

        if (ativaNicole && PlayerStatus.isNicoleActive()) {
            GetComponent<AtivadorDeAliado>().ativarNicole();
        }
        if (ativaAlex && PlayerStatus.isAlexActive()) {
            GetComponent<AtivadorDeAliado>().ativarAlex();
        }
        StartObjeto();
        anim = GetComponent<Animator>();

        if (GameObject.Find("PauseMenu") != null) {
            pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        }

    }
	
    protected virtual void attVoice() {
        gameManager.setCVoice(Array.IndexOf(gameManager.nomes, conversa.nomes[cDialogue]));
    }

    protected virtual void Update () {
        UpdateObjeto();
        if(pauseMenu == null || (conversando == true && !pauseMenu.isPaused)) {
            if (GameManager.getFundoPreto() != null) {
                GameManager.getFundoPreto().SetActive(fundoPreto);
            }
            if (cDelay <= 0) {
                progresso = progresso + conversa.velocidade * Time.deltaTime;
            } else {
                cDelay -= Time.deltaTime;
                if(cDelay <= 0) {
                    progresso++;
                }
            }

            if (progresso > conversa.getFala(cDialogue).Length) {
                progresso = conversa.getFala(cDialogue).Length;
            }
            conversationText.text = conversa.getFala(cDialogue).Substring(0, (int) progresso);

            if (conversa.getFala(cDialogue)[(int) progresso - 1] == ',' && cDelay <= 0) {
                cDelay = DELAY_VIRGULA;
            } else if ((conversa.getFala(cDialogue)[(int) progresso - 1] == '.' || conversa.getFala(cDialogue)[(int)progresso - 1] == '!' || conversa.getFala(cDialogue)[(int)progresso - 1] == '?') && cDelay <= 0) {
                cDelay = DELAY_PONTO;
            }

            if(progresso > cSoundSpeed && !pulou) {
                playSound();
                cSoundSpeed += soundSpeed;
            }

            if (Input.GetMouseButton(0) && cTempoMinimoAtePular > tempoMinimoAtePular) {
                cDelay = 0;
                cTempoMinimoAtePular = 0;
                if (progresso < conversa.getFala(cDialogue).Length) {
                    progresso = conversa.getFala(cDialogue).Length;
                    conversationText.text = conversa.getFala(cDialogue).Substring(0, (int) progresso);
                    pulou = true;
                } else {
                    pulou = false;
                    conversationText.text = "";
                    progresso = 1;
                    cSoundSpeed = 0;
                    if (cDialogue < conversa.falas.Length - 1) {
                        cDialogue++;
                        attVoice();
                        onDialogueChange();
                        conversationBox.transform.Find("imagem").GetComponent<Image>().sprite = conversa.sprites[cDialogue];
                        nomesText.text = conversa.getNome(cDialogue);
                       
                        attIndice();
                    } else {
                        cDialogue = 0;
                        conversando = false;

                        if (GameObject.Find("Player") != null) {
                            GameObject.Find("Player").GetComponent<Player>().setFreeze(false);
                        }
                        hideDialogueBox();
                        if (sp != null) {
                            sp.sprite = spriteAntesDeConversar;
                        }
                        return;
                    }
                }
            } else {
                cTempoMinimoAtePular += Time.deltaTime;
            }
        }
	}

    protected virtual void onDialogueChange() {

    }

    protected virtual void playSound() {
        gameManager.playVoiceSound();
    }

    public bool jaFoiExecutada()
    {
        Dictionary<int, bool> conversasExecutadas = ConversaScriptable.getConversasExecutadas();
        if (conversasExecutadas.ContainsKey(conversa.id))
        {
            return conversasExecutadas[conversa.id];
        } else
        {
            return false;
        }

    }
       

    public override void interagir() {
        if (parar) {
            GetComponent<NPC>().parar = true;
        }

        if (anim != null && !parametroAnimacao.Equals("")) {
            anim.SetBool(parametroAnimacao, true);
        }

        conversa = conversa.att();

        ConversaScriptable.execute(conversa.id);

        if (GameManager.getConversationBox() != null) {
            conversationBox = GameManager.getConversationBox();
        }
        conversationText = conversationBox.transform.Find("conversationTxt").GetComponent<Text>();
        conversationBox.transform.Find("imagem").GetComponent<Image>().sprite = conversa.sprites[cDialogue];
        showDialogueBox();
        nomesText = GameObject.Find("conversationNome").GetComponent<Text>();
        indiceText = GameObject.Find("conversationIndice").GetComponent<Text>();
        nomesText.text = conversa.getNome(cDialogue);
        conversando = true;

        if (GameObject.Find("Player") != null) {
            GameObject.Find("Player").GetComponent<Player>().setFreeze(true);
        }
        
        attIndice();

        attVoice();

        sp = GetComponent<SpriteRenderer>();
        if (sp != null) {
            spriteAntesDeConversar = sp.sprite;
            if (spriteEnquantoConversa != null) {
                sp.sprite = spriteEnquantoConversa;
            }
        }
    }

    private void attIndice() {
        indiceText.text = (cDialogue + 1) + " de " + conversa.falas.Length;
    }

    private void showDialogueBox() {
        conversationBox.SetActive(true);
    }

    protected virtual void hideDialogueBox() {
        if(proximaMusicaDeFundo != null && conversa.proximaConversa == null) {
            GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
            if (caixaDeSom != null) {
                caixaDeSom.GetComponent<MusicaDeFundo>().setBackground(proximaMusicaDeFundo);
                caixaDeSom.GetComponent<MusicaDeFundo>().playBg();
            }
        }

        if (parar) {
            GetComponent<NPC>().parar = false;
        }

        conversationBox.SetActive(false);
        if (fundoPreto) {
            GameManager.getFundoPreto().GetComponent<FadeOut>().fadeOut();
        }
        if (cutscene != null) {
            GameManager.playCutscene(cutscene);
        }
        if (ativaNicole) {
            GetComponent<AtivadorDeAliado>().ativarNicole();
        }
        if (ativaAlex) {
            GetComponent<AtivadorDeAliado>().ativarAlex();
        }
        if(objetivo != -1) {
            Objetivo obj = DicionarioObjetivos.getObjetivoByIndex(objetivo);
            PlayerStatus.addObjective(obj);
        }
        if(avancarObjetivo != -1) {
            PlayerStatus.fowardObjective(avancarObjetivo, 1);
        }
        if(avancarObjetivo2 != -1) {
            PlayerStatus.fowardObjective(avancarObjetivo2, 1);
        }

        if(conversa.progressoAoTerminar != -1) {
            if (PlayerStatus.getProgresso() < conversa.progressoAoTerminar || conversa.voltaProgresso) {
                PlayerStatus.setProgresso(conversa.progressoAoTerminar);
            }
        }
        if (chaveBob) {
            PlayerStatus.acharChaveBob();
        }

        this.enabled = !desabilitarAposUsar;
        if(anim != null && !parametroAnimacao.Equals("")) {
            anim.SetBool(parametroAnimacao, false);
        }

        if (chaveMendigo) {
            PlayerStatus.acharChaveMendigo();

            GameManager.showMessage("Por baixo dos lençóis sujos, você encontra o objeto que estava procurando.", "Chave encontrada");
            GameObject.Find("Player").GetComponent<Player>().setFreeze(true);
            ChaveMendigo.mendigo.SetActive(true);
            ChaveMendigo.avisoMendigo.SetActive(true);
        }

        if (!destino.Equals("")) {
            PlayerStatus.setProximaBatalha(proximaBatalha);
            PlayerStatus.setUltimaCena(SceneManager.GetActiveScene().name);
            PlayerStatus.setUltimoPlayerX(GameObject.Find("Player").transform.position.x);
            GameManager.setPlayerOlhandoEsquerda(playerOlhandoEsquerda);
            if (playerX != -1) {
                GameManager.setPlayerX(playerX);
            }
            if (!destino.Equals("Combate"))
            {
                SceneManager.LoadScene(destino, LoadSceneMode.Single);
            } else {
                GameObject.Find("Player").GetComponent<Player>().setFreeze(true);
                Instantiate(gameManager.fight, new Vector3(), Quaternion.identity);
            }
        }

        if (salvar)
        {
            PlayerStatus.setUltimoPlayerX(GameObject.Find("Player").transform.position.x);
            PlayerStatus.save();
            GameManager.showMessage("O jogo foi salvo com sucesso.", "Jogo salvo");
            GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
            if (caixaDeSom != null)
            {
                caixaDeSom.GetComponent<MusicaDeFundo>().playSound(52);
            }
        }
        conversa = conversa.att();

        
    }

    public bool getAutomatico() {
        return conversa.automatico;
    }

    public int getProgressoMinimo() {
        return conversa.progressoMinimo;
    }

    public int getProgressoMaximo() {
        if(conversa.progressoMaximo == -1) {
            return int.MaxValue;
        }
        return conversa.progressoMaximo;
    }

    public int getNivelMinimoDash()
    {
        return this.nivelMinimoDash;
    }

    public int getNivelMaximoDash()
    {
        if (nivelMaximoDash == -1)
        {
            return int.MaxValue;
        }
        return this.nivelMaximoDash;
    }

    public int getNivelMinimoProjetil()
    {
        return this.nivelMinimoProjetil;
    }

    public int getNivelMaximoProjetil()
    {
        if (nivelMaximoProjetil == -1)
        {
            return int.MaxValue;
        }
        return this.nivelMaximoProjetil;
    }

    public int getNivelMinimoEspecial()
    {
        return this.nivelMinimoEspecial;
    }

    public int getNivelMaximoEspecial()
    {
        if (nivelMaximoEspecial == -1)
        {
            return int.MaxValue;
        }
        return this.nivelMaximoEspecial;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour {
    public AudioClip[] vozes;
    public String[] nomes;
    public float vidaStep;
    public float velocidadeAtaqueStep;
    public float velocidadeMovimentoStep;
    public float danoStep;
    public Sprite coracao;
    public Sprite coracaoApagado;
    public Sprite shurikenVermelha;
    public Sprite shurikenCinza;
    public Sprite bolaDesativada;
    public Sprite bolaAtivada;
    public bool isIntroducao = false;
    public GameObject fight;

    public int debugInitialProgress;

    private static float precoResetar = 100;

    public static bool isLevelUp;
    public static bool isNovaPagina;

    private int cVoice;
    private static bool mudouProgressoDebug;
    public static bool objetivosAberto;
    private Text dinheirosTxt;
    private static Transform status;
    private GameObject arvoreDeStatus;
    private static Text objetivosTxt;
    private static GameObject player;
    private static GameObject dialogueBox;
    private static GameObject conversationBox;
    private static GameObject fundoPreto;
    private static GameObject objetivos;
    private static GameObject canvasPF1;
    private static Trabalho proximoTrabalho;
    private static PauseMenu pauseMenu;
    private static bool wasPlayingLastFrame;

    private static float playerX;
    private static bool playerOlhandoEsquerda;
    public static bool isPlayingCutscene;

    private static GameObject[,] barras;
    private static GameObject[,] bolas;
    private static Text pontosTxt;
    private static Text precoResetarTxt;
    private static MusicaDeFundo caixaDeSom;

    void Awake() {
        if (debugInitialProgress != 0 && !mudouProgressoDebug) {
            mudouProgressoDebug = true;
            //PlayerStatus.setProgresso(debugInitialProgress);
        }

        try {
            caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
        } catch (NullReferenceException) {

        }

        canvasPF1 = GameObject.Find("CanvasPF 1");

        player = GameObject.Find("Player");
        if (playerX != 0 && player != null) {
            player.transform.position = new Vector3(playerX, player.transform.position.y, player.transform.position.z);
        }

        bolaAtivada = Resources.Load<Sprite>("bolaAtivada");

        if(GameObject.Find("PauseMenu") != null)
            pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();

        GameObject nicole = GameObject.Find("Nicole");
        if (nicole != null)
            nicole.SetActive(PlayerStatus.isNicoleActive());

        GameObject alex = GameObject.Find("Alex");
        if (alex != null)
            alex.SetActive(PlayerStatus.isAlexActive());

        if (GameObject.Find("Status") != null)
        {
            status = GameObject.Find("Status").transform;
            status.gameObject.SetActive(false);
        }

        if (GameObject.Find("ObjetivosTxt") != null)
        {
            objetivosTxt = GameObject.Find("ObjetivosTxt").GetComponent<Text>();
            attObjetivos();
        }

        dialogueBox = GameObject.Find("dialogueBox");
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        conversationBox = GameObject.Find("conversationBox");
        if (conversationBox != null)
            conversationBox.SetActive(false);

        fundoPreto = GameObject.Find("FundoPreto");
        if (fundoPreto != null)
            fundoPreto.SetActive(false);
                
        objetivos = GameObject.Find("Objetivos");
        arvoreDeStatus = GameObject.Find("ArvoreDeStatus");        

        barras = new GameObject[4, 5];
        for (int i = 0; i < 5; i++) {
            barras[0, i] = GameObject.Find("BarraVida" + (i + 1));
        }
        for (int i = 0; i < 5; i++) {
            barras[1, i] = GameObject.Find("BarraDano" + (i + 1));
        }
        for (int i = 0; i < 5; i++) {
            barras[2, i] = GameObject.Find("BarraVelAtaque" + (i + 1));
        }
        for (int i = 0; i < 3; i++) {
            barras[3, i] = GameObject.Find("BarraVelMov" + (i + 1));
        }

        bolas = new GameObject[4, 5];
        for (int i = 0; i < 5; i++) {
            bolas[0, i] = GameObject.Find("Vida" + (i + 1));
        }
        for (int i = 0; i < 5; i++) {
            bolas[1, i] = GameObject.Find("Dano" + (i + 1));
        }
        for (int i = 0; i < 5; i++) {
            bolas[2, i] = GameObject.Find("VelAtaque" + (i + 1));
        }
        for (int i = 0; i < 3; i++) {
            bolas[3, i] = GameObject.Find("VelMov" + (i + 1));
        }

        if(GameObject.Find("pontosTxt") != null)
            pontosTxt = GameObject.Find("pontosTxt").GetComponent<Text>();

        if(arvoreDeStatus != null)
            arvoreDeStatus.SetActive(false);

        /*
        //txtDinheiros = GameObject.Find("txtDinheiros").GetComponent<Text>();
        //objetivos.SetActive(!isOnStatus);

        SlideX slideStatus = GameObject.Find("painelStatus").GetComponent<SlideX>();
        if (statusPainelAberto) {
            slideStatus.abrirInstantaneo();
        } else {
            slideStatus.fecharInstantaneo();
        }
        */

       
    }

    private void Start() {
        if (!isIntroducao)
        {
            if (isLevelUp)
            {
                isLevelUp = false;
                levelUp();
            }

            if (!objetivosAberto)
            {
                objetivos.GetComponent<SlideX>().fecharInstantaneo();
            }

            if (playerOlhandoEsquerda)
            {
                player.GetComponent<Player>().lookLeft();
            } else
            {
                player.GetComponent<Player>().lookRight();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (!isIntroducao)
        {
            attObjetivos();
        }

        if (PlayerStatus.getProgresso() > 4) {
            PlayerStatus.horario += Time.deltaTime;
        } else if(PlayerStatus.getProgresso() > -1)
        {
            if (PlayerStatus.horario < Sol.duracaoMadrugada)
                PlayerStatus.horario += Time.deltaTime;
            else
                PlayerStatus.horario = Sol.duracaoMadrugada;
        }

        if (isPlayingCutscene) {
            GameObject videoO = GameObject.Find("Cutscene");
            VideoPlayer video = videoO.GetComponent<VideoPlayer>();
            
            if (pauseMenu.isPaused) {
                video.Pause();
                wasPlayingLastFrame = false;
            } else if (!wasPlayingLastFrame) {
                video.Play();
                wasPlayingLastFrame = true;
            }

            if (video.isPlaying) {
                fundoPreto.SetActive(false);
            }

            if ((video.isPrepared && !video.isPlaying && !pauseMenu.isPaused)) {
                showCanvas1();
                Destroy(videoO);
                isPlayingCutscene = false;
                player.GetComponent<Player>().setFreeze(false);
            }

        }

    }

    public void levelUp() {
        PlayerStatus.pontosDeHabilidade++;
        if (isNovaPagina) {
            caixaDeSom.playSound(6);
            showMessage("Há novas oportunidades de emprego e estudos.\nVocê também ganhou um novo ponto de status!", "Novidades");
            PlayerStatus.setPaginasDesbloquadas(PlayerStatus.getPaginasDesbloquadas() + 1);
            isNovaPagina = false;
        } else {
            showMessage("Você ganhou um novo ponto de status!\nVá para a árvore de status para usá-lo.", "Novo Ponto");
        }
    }

    public static void abrirObjetivos() {
        if (!objetivosAberto) {
            objetivos.GetComponent<SlideX>().mover();
        }
    }

    public static void attObjetivos() {
        ArrayList pObjetivos = PlayerStatus.getObjetivos();
        if (pObjetivos.Count > 0) {
            objetivosTxt.text = "";
            for (int i = 0; i < pObjetivos.Count; i++) {
                if(i != 0) {
                    objetivosTxt.text += "\n";
                }
                if (((Objetivo)pObjetivos[i]).getMax() > 1) {
                    objetivosTxt.text += ((Objetivo)pObjetivos[i]).getContent() + " " + ((Objetivo)pObjetivos[i]).getCurrent() + "/" + ((Objetivo)pObjetivos[i]).getMax();
                } else {
                    objetivosTxt.text += ((Objetivo)pObjetivos[i]).getContent();
                }
            }
        } else {
            objetivosTxt.text = "Nenhum objetivo";
        }
    }

    public void attStatus() {
        Text dinheiro = status.GetChild(0).GetComponent<Text>();
        dinheiro.text = PlayerStatus.getDinheiros().ToString("0.00");
        Transform conhecimentos = status.GetChild(1);
        for (int i = 0; i < conhecimentos.childCount; i++) {
            if (PlayerStatus.getInteligencia()[i]) {
                conhecimentos.GetChild(i).GetChild(0).GetComponent<Text>().color = new Color(0.81f, 0.81f, 0.81f, 1);
                conhecimentos.GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(true);
            } else {
                conhecimentos.GetChild(i).GetChild(0).GetComponent<Text>().color = new Color(0.43f, 0.43f, 0.43f, 0.5f);
            }
        }

        Transform statusCombate = status.GetChild(2);
        int[] niveis = new int[4];
        niveis[0] = PlayerStatus.upsVida;
        niveis[1] = PlayerStatus.upsVelocidade;
        niveis[2] = PlayerStatus.upsVelocidadeAtaque;
        niveis[3] = PlayerStatus.upsDano;
        for (int j = 0; j < 4; j++) {
            for (int i = 0; i < 5; i++) {
                Image imagem = statusCombate.GetChild(j).GetChild(i).GetComponent<Image>();
                if (niveis[j] > i) {
                    imagem.sprite = coracao;
                } else {
                    imagem.sprite = coracaoApagado;
                }
            }
        }

        niveis[0] = PlayerStatus.nivelProjetil;
        niveis[1] = PlayerStatus.nivelDash;
        niveis[2] = PlayerStatus.nivelEspecial;
        for (int j = 0; j < 3; j++) {
            for (int i = 0; i < 3; i++) {
                Image imagem = statusCombate.GetChild(4 + j).GetChild(i).GetComponent<Image>();
                if (niveis[j] > i) {
                    imagem.sprite = shurikenVermelha;
                } else {
                    imagem.sprite = shurikenCinza;
                }
            }
        }
    }

    public static void setPlayerX(float x) {
        playerX = x;
    }

    public static void setPlayerOlhandoEsquerda(bool x)
    {
        playerOlhandoEsquerda = x;
    }

    public static GameObject getConversationBox() {
        return conversationBox;
    }

    public static GameObject getFundoPreto() {
        return fundoPreto;
    }

    public static GameObject getObjetivos() {
        return objetivos;
    }

    public static void showMessage(string msg) {
        showMessage(msg, "Aviso");
    }

    public static void showMessage(string msg, string titulo) {
        dialogueBox.SetActive(true);
        dialogueBox.GetComponent<IncreaseScale>().increase();
        player.GetComponent<Player>().setFreeze(true);
        dialogueBox.transform.Find("dialogueTxt").GetComponent<Text>().text = msg;
        GameObject.Find("titulo").GetComponent<Text>().text = titulo;
    }

    public static Trabalho getProximoTrabalho() {
        return GameManager.proximoTrabalho;
    }

    public static void setProximoTrabalho(Trabalho proximoTrabalho) {
        GameManager.proximoTrabalho = proximoTrabalho;
    }

    public static void playCutscene(VideoClip cutscene) {
        VideoPlayer vp = GameObject.Find("Cutscene").GetComponent<VideoPlayer>();
        vp.clip = cutscene;
        vp.Play();

        player.GetComponent<Player>().setFreeze(true);
        isPlayingCutscene = true;

        hideCanvas1();
    }

    private static void hideCanvas1() {
        canvasPF1.SetActive(false);
    }

    private static void showCanvas1() {
        canvasPF1.SetActive(true);
    }

    public void attArvoreHabilidades() {
        if(PlayerStatus.getDinheiros() < precoResetar) {

            GameObject btnResetar = GameObject.Find("BotaoResetar");
            btnResetar.GetComponent<Button>().interactable = false;
            btnResetar.transform.GetChild(0).GetComponent<Text>().color = new Color(0.43f, 0.43f, 0.43f, 0.5f);
            btnResetar.transform.GetChild(1).GetComponent<Text>().color = new Color(0.43f, 0.43f, 0.43f, 0.5f);
        }

        pontosTxt.text = ":" + PlayerStatus.pontosDeHabilidade;

        int[] niveis = new int[4];
        niveis[0] = PlayerStatus.upsVida;
        niveis[1] = PlayerStatus.upsDano;
        niveis[2] = PlayerStatus.upsVelocidadeAtaque;
        niveis[3] = PlayerStatus.upsVelocidade;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < niveis[i]; j++) {
                barras[i, j].GetComponent<Image>().color = new Color(1, 1, 1);
                bolas[i, j].GetComponent<Image>().sprite = bolaAtivada;
                bolas[i, j].GetComponent<Image>().color = new Color(0.74f, 0.74f, 0.74f);
            }
        }
    }

    public void melhorarVida() {
        if (PlayerStatus.pontosDeHabilidade > 0 && PlayerStatus.upsVida < 5) {
            PlayerStatus.setVida(PlayerStatus.getVida() + vidaStep);
            PlayerStatus.upsVida++;
            PlayerStatus.pontosDeHabilidade--;
            attArvoreHabilidades();
        }
    }

    public void melhorarVelocidadeDeMovimento() {
        if (PlayerStatus.pontosDeHabilidade > 0 && PlayerStatus.upsVelocidade < 3) {
            PlayerStatus.setVelocidade(PlayerStatus.getVelocidade() + velocidadeMovimentoStep);
            PlayerStatus.upsVelocidade++;
            PlayerStatus.pontosDeHabilidade--;
            attArvoreHabilidades();
        }
    }

    public void melhorarVelocidadeDeAtaque() {
        if (PlayerStatus.pontosDeHabilidade > 0 && PlayerStatus.upsVelocidadeAtaque < 5) {
            PlayerStatus.setVelocidadeDeAtaque(PlayerStatus.getVelocidadeDeAtaque() + velocidadeAtaqueStep);
            PlayerStatus.upsVelocidadeAtaque++;
            PlayerStatus.pontosDeHabilidade--;
            attArvoreHabilidades();
        }
    }

    public void melhorarDano() {
        if (PlayerStatus.pontosDeHabilidade > 0 && PlayerStatus.upsDano < 5) {
            PlayerStatus.setDano(PlayerStatus.getDano() + danoStep);
            PlayerStatus.upsDano++;
            PlayerStatus.pontosDeHabilidade--;
            attArvoreHabilidades();
        }
    }

    public void setCVoice(int cVoice) {
        this.cVoice = cVoice;
    }

    public void playVoiceSound() {
        if (caixaDeSom != null) {
            caixaDeSom.playSound(vozes[cVoice]);
        }
    }

    public void resetarPontos() {
        PlayerStatus.setDinheiros(PlayerStatus.getDinheiros() - precoResetar);

        PlayerStatus.setVelocidade(1);
        PlayerStatus.setVida(1);
        PlayerStatus.setVelocidadeDeAtaque(1);
        PlayerStatus.setDano(1);

        int ups = 0;

        ups += PlayerStatus.upsVelocidade;
        ups += PlayerStatus.upsVelocidadeAtaque;
        ups += PlayerStatus.upsVida;
        ups += PlayerStatus.upsDano;

        PlayerStatus.upsVelocidade = 0;
        PlayerStatus.upsVelocidadeAtaque = 0;
        PlayerStatus.upsVida = 0;
        PlayerStatus.upsDano = 0;

        PlayerStatus.pontosDeHabilidade += ups;

        limparArvore();
    }

    private void limparArvore() {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 5; j++) {
                if (i == 3 && j == 3) {
                    break;
                }
                Debug.Log(i + " " + j);
                barras[i, j].GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
                bolas[i, j].GetComponent<Image>().sprite = bolaDesativada;
                bolas[i, j].GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
            }
        }

        attArvoreHabilidades();
    }
}
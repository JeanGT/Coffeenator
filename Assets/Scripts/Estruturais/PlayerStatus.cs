using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerStatus {
    public static float horario = 1;

    public static int projetil;

    public static string nome = "Gustavo";

    public static int nivelProjetil;
    public static int nivelDash;
    public static int nivelEspecial;

    public static bool tutorialPC;
    public static bool tutorialCombate;

    private static int progressoAoEncontrarAsChaves = 18;

    private const string criptoKey = "uEGIw9ufRMofedP3LfC6jE9iLdE3bGeh";

    public static int pontosDeHabilidade = 0;

    public static int upsVelocidade = 0;
    public static int upsVida = 0;
    public static int upsVelocidadeAtaque = 0;
    public static int upsDano = 0;

    private static float velocidade = 1;
    private static float vida = 1;
    private static float velocidadeAtaque = 1;
    private static float dano = 1.2f; //default: 1.2f

    private static bool[] inteligencia = new bool[10];
    private static ArrayList objetivos = new ArrayList();
    private static int progresso = -1; // default: -1

    private static float dinheiros;
    private static float dinheirosAreceber;
    private static int proximaBatalha = -1;
    private static string ultimaCena;
    private static float ultimoPlayerX;

    private static bool temNicole;
    private static bool temAlex;
    private static bool temChaveMendigo;
    private static bool temChaveBob;
    private static bool programou;
    private static bool interruptorQuarto;
    private static bool interruptorCasa;

    private static int paginasDesbloquadas;

    private static int avisoMendigo;

    public static float getVida()
    {
        return vida;
    }

    public static float getVelocidade()
    {
        return velocidade;
    }

    public static float getDano()
    {
        return dano;
    }

    public static float getVelocidadeDeAtaque()
    {
        return velocidadeAtaque;
    }

    public static void setVida(float x)
    {
        vida = x;
    }

    public static void setVelocidade(float x)
    {
        velocidade = x;
    }

    public static void setDano(float x)
    {
        dano = x;
    }

    public static void setAvisoMendigo(int novoAviso)
    {
        avisoMendigo = novoAviso;
    }

    public static void setVelocidadeDeAtaque(float x)
    {
        velocidadeAtaque = x;
    }

    public static void acharChaveMendigo() {
        temChaveMendigo = true;
        if (temChaveBob) {
            duasChaves();
        }
    }

    public static void acharChaveBob() {
        temChaveBob = true;
        if (temChaveMendigo) {
            duasChaves();
        }
    }

    private static void duasChaves()
    {
        PlayerStatus.setProgresso(progressoAoEncontrarAsChaves);
        addObjective(DicionarioObjetivos.getObjetivoByIndex(10));
    }

    public static void setInterruptorQuarto(bool x) {
        interruptorQuarto = x;
    }

    public static void setInterruptorCasa(bool x) {
        interruptorCasa = x;
    }

    public static bool isInterruptorCasa() {
        return interruptorCasa;
    }

    public static bool isInterruptorQuarto() {
        return interruptorQuarto;
    }

    public static bool isChaveMendigo() {
        return temChaveMendigo;
    }

    public static bool isChaveBob() {
        return temChaveBob;
    }

    public static void ativarNicole() {
        temNicole = true;
    }

    public static void ativarAlex() {
        temAlex = true;
    }

    public static void desativarNicole()
    {
        temNicole = false;
    }

    public static void desativarAlex()
    {
        temAlex = false;
    }

    public static bool isNicoleActive() {
        return temNicole;
    }

    public static bool isAlexActive() {
        return temAlex;
    }

    public static void setUltimoPlayerX(float ultimoPlayerX) {
        PlayerStatus.ultimoPlayerX = ultimoPlayerX;
    }

    public static float getUltimoPlayerX() {
        return PlayerStatus.ultimoPlayerX;
    }

    public static void setUltimaCena(string ultimaCena) {
        PlayerStatus.ultimaCena = ultimaCena;
    }

    public static string getUltimaCena() {
        return PlayerStatus.ultimaCena;
    }

    public static void fowardObjective(int objIndex, int steps) {
        for(int i = 0; i < objetivos.Count; i++) {
            if(DicionarioObjetivos.getIndexByObjetivo((Objetivo) objetivos[i]) == objIndex) {
                ((Objetivo)objetivos[i]).foward(steps);
            }
        }
    }

    public static int getProgresso() {
        return progresso;
    }

    public static void setProgresso(int progresso) {
        PlayerStatus.progresso = progresso;
        Debug.Log("<color=blue> Progresso: </color>" + progresso);
    }


    public static void addObjective(Objetivo obj) {
        GameManager.showMessage(obj.getContent(), "Novo Objetivo");
        GameManager.abrirObjetivos();
        objetivos.Add(obj);
    }

    public static void removeObjective(Objetivo obj) {
        objetivos.Remove(obj);
    }

    public static ArrayList getObjetivos() {
        return objetivos;
    }

    public static float getDinheiros() {
        return dinheiros;
    }

    public static int getPaginasDesbloquadas()
    {
        return paginasDesbloquadas;
    }

    public static int getAvisoMendigo()
    {
        return avisoMendigo;
    }

    public static void setDinheiros(float dinheiros) {
        if(dinheiros < 0) {
            dinheiros = 0;
        }
        PlayerStatus.dinheiros = dinheiros;
    }

    public static float getDinheirosAReceber() {
        return dinheirosAreceber;
    }

    public static void setDinheirosAReceber(float dinheiros) {
        PlayerStatus.dinheirosAreceber = dinheiros;
    }

    public static int getProximaBatalha() {
        return proximaBatalha;
    }

    public static void setProximaBatalha(int proximaBatalha) {
        PlayerStatus.proximaBatalha = proximaBatalha;
    }

    public static bool[] getInteligencia() {
        return inteligencia;
    }

    public static void setInteligencia(int index, bool value) {
        inteligencia[index] = value;
    }

    public static bool isProgramou() {
        return programou;
    }

    public static void setProgramou(bool programou) {
        PlayerStatus.programou = programou;
    }

    public static void setPaginasDesbloquadas(int paginasDesbloqueadas)
    {
        PlayerStatus.paginasDesbloquadas = paginasDesbloqueadas;
    }

    public static void morrer() {
        pontosDeHabilidade = 0;

        upsVelocidade = 0;
        upsVida = 0;
        upsVelocidadeAtaque = 0;
        upsDano = 0;

        horario = 1;

        projetil = 0;
        nivelDash = 0;
        nivelEspecial = 0;
        nivelProjetil = 0;

        velocidade = 1;
        vida = 1;
        velocidadeAtaque = 1;
        dano = 1.2f;

        inteligencia = new bool[10];
        objetivos = new ArrayList();
        progresso = -1;

        dinheiros = 0;
        dinheirosAreceber = 0;
        proximaBatalha = -1;
        ultimaCena = "";
        ultimoPlayerX = 0;
        paginasDesbloquadas = 0;

        temNicole = false;
        temAlex = false;
        temChaveMendigo = false;
        temChaveBob = false;
        programou = false;
        interruptorCasa = false;
        interruptorQuarto = false;
        avisoMendigo = 0;
    }

    public static void excluirSave() {
        PlayerPrefs.SetString("save", "sem save");
    }

    public static void save()
    {
        string save = "";

        save += horario + "|";
        save += projetil + "|";
        save += nivelDash + "|";
        save += nivelProjetil + "|";
        save += nivelEspecial + "|";
        save += velocidade + "|";
        save += vida + "|";
        save += velocidadeAtaque + "|";
        save += dano + "|";

        for (int i = 0; i < 10; i++) {
            int cInteligencia = 0;
            if (inteligencia[i]) {
                cInteligencia = 1;
            }
            save += cInteligencia + "|";
        }

        save += progresso + "|";
        save += dinheiros + "|";
        save += ultimaCena + "|";
        save += ultimoPlayerX + "|";

        int cTemNicole = 0, cTemAlex = 0, cTemChaveMendigo = 0, cTemChaveBob = 0, cProgramou = 0, cInterruptorCasa = 0, cInterruptorQuarto = 0, cTutorialPC = 0, cTutorialCombate = 0;
        if (tutorialCombate) {
            cTutorialCombate = 1;
        }
        if (tutorialPC) {
            cTutorialPC = 1;
        }
        if (temNicole) {
            cTemNicole = 1;
        }
        if (temAlex) {
            cTemAlex = 1;
        }
        if (temChaveMendigo) {
            cTemChaveMendigo = 1;
        }
        if (temChaveBob) {
            cTemChaveBob = 1;
        }
        if (programou) {
            cProgramou = 1;
        }
        if (interruptorCasa) {
            cInterruptorCasa = 1;
        }
        if (interruptorQuarto) {
            cInterruptorQuarto = 1;
        }

        save += cTutorialPC + "|";
        save += cTutorialCombate + "|";
        save += cTemNicole + "|";
        save += cTemAlex + "|";
        save += cTemChaveMendigo + "|";
        save += cTemChaveBob + "|";
        save += cProgramou + "|";
        save += cInterruptorCasa + "|";
        save += cInterruptorQuarto + "|";

        save += avisoMendigo + "|";

        save += SceneManager.GetActiveScene().name + "|";

        save += pontosDeHabilidade + "|";

        save += nome + "|";

        save += paginasDesbloquadas + "|";

        save += ConversaScriptable.maiorId + "|";

        save += upsDano + "|";
        save += upsVelocidade + "|";
        save += upsVelocidadeAtaque + "|";
        save += upsVida + "|";

        if(ChaveMendigo.derrotou)
            save += 1 + "|";
        else
            save += 0 + "|";

        if (objetivos.Count > 0)
        {
            save += DicionarioObjetivos.getIndexByObjetivo((Objetivo)objetivos[0]) + "|";
            save += ((Objetivo)objetivos[0]).getCurrent() + "|";
        } else
        {
            save += -1 + "|";
            save += -1 + "|";
        }

        if (objetivos.Count > 1)
        {
            save += DicionarioObjetivos.getIndexByObjetivo((Objetivo)objetivos[1]) + "|";
            save += ((Objetivo)objetivos[1]).getCurrent() + "|";
        }
        else
        {
            save += -1 + "|";
            save += -1 + "|";
        }
        
        Dictionary<int, bool> conversasExecutadas = ConversaScriptable.getConversasExecutadas();

        for(int i = 0; i <= ConversaScriptable.maiorId; i++) {
            bool executou = false;
            conversasExecutadas.TryGetValue(i, out executou);
            if (executou) {
                save += "1|";
            } else {
                save += "0|";
            }
        }

        string[] x = save.Split('|');

        string saveCriptografado = CriptografarString.Encrypt(save, criptoKey);

        PlayerPrefs.SetString("save", saveCriptografado);
        PlayerPrefs.Save();
    }

    public static void load(AudioClip[] musicas) {
        string[] savedValues = CriptografarString.Decrypt(PlayerPrefs.GetString("save"), criptoKey).Split('|');

        horario = float.Parse(savedValues[0]);

        projetil = int.Parse(savedValues[1]);
        nivelDash = int.Parse(savedValues[2]);
        nivelProjetil = int.Parse(savedValues[3]);
        nivelEspecial = int.Parse(savedValues[4]);

        velocidade = float.Parse(savedValues[5]);
        vida = float.Parse(savedValues[6]);
        velocidadeAtaque = float.Parse(savedValues[7]);
        dano = float.Parse(savedValues[8]);

        inteligencia = new bool[10];
        for (int i = 0; i < 10; i++) {
            inteligencia[i] = int.Parse(savedValues[9 + i]) == 1;
        }

        progresso = int.Parse(savedValues[19]);
        dinheiros = float.Parse(savedValues[20]);
        ultimaCena = savedValues[21];
        ultimoPlayerX = float.Parse(savedValues[22]);

        tutorialPC = int.Parse(savedValues[23]) == 1;
        tutorialCombate = int.Parse(savedValues[24]) == 1;
        temNicole = int.Parse(savedValues[25]) == 1;
        temAlex = int.Parse(savedValues[26]) == 1;
        temChaveMendigo = int.Parse(savedValues[27]) == 1;
        temChaveBob = int.Parse(savedValues[28]) == 1;
        programou = int.Parse(savedValues[29]) == 1;
        interruptorCasa = int.Parse(savedValues[30]) == 1;
        interruptorQuarto = int.Parse(savedValues[31]) == 1;
        avisoMendigo = int.Parse(savedValues[32]);

        pontosDeHabilidade = int.Parse(savedValues[34]);

        nome = savedValues[35];

        paginasDesbloquadas = int.Parse(savedValues[36]);

        ConversaScriptable.maiorId = int.Parse(savedValues[37]);

        upsDano = int.Parse(savedValues[38]);
        upsVelocidade = int.Parse(savedValues[39]);
        upsVelocidadeAtaque = int.Parse(savedValues[40]);
        upsVida = int.Parse(savedValues[41]);
        ChaveMendigo.derrotou = int.Parse(savedValues[42]) == 1;

        int indexObjetivo1 = int.Parse(savedValues[43]);
        int currentObjetivo1 = int.Parse(savedValues[44]);
        int indexObjetivo2 = int.Parse(savedValues[45]);
        int currentObjetivo2 = int.Parse(savedValues[46]);

        if (indexObjetivo1 != -1)
        {
            Objetivo obj1 = DicionarioObjetivos.getObjetivoByIndex(indexObjetivo1);
            obj1.foward(currentObjetivo1);
            objetivos.Add(obj1);
        }

        if (indexObjetivo2 != -1)
        {
            Objetivo obj2 = DicionarioObjetivos.getObjetivoByIndex(indexObjetivo2);
            obj2.foward(currentObjetivo2);
            objetivos.Add(obj2);
        }

        Dictionary<int, bool> conversasExecutadas = new Dictionary<int, bool>();

        for (int i = 0; i <= ConversaScriptable.maiorId; i++) {
            bool executou = int.Parse(savedValues[47 + i]) == 1;
            conversasExecutadas.Add(i, executou);
        }

        ConversaScriptable.setConversasExecutadas(conversasExecutadas);


        //todo
        int musicaIndex;

        if(progresso < 7)
        {
            musicaIndex = 2;
        } else
        {
            musicaIndex = 3;
        }

        GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().setBackground(musicas[musicaIndex]);
        GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>().playBg();
        SceneManager.LoadScene(savedValues[33], LoadSceneMode.Single);
        GameManager.setPlayerX(ultimoPlayerX);
        GameManager.setPlayerOlhandoEsquerda(false);
        if(horario > Sol.duracaoDia)
        {
            NPCSpawn.npcsDia = true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nova Conversa", menuName = "Conversa")]
public class ConversaScriptable : ScriptableObject {
    public string[] falas;
    public Sprite[] sprites;
    public string[] nomes;
    public float velocidade = 40;
    public bool voltaProgresso;
    public bool automatico;
    public int progressoAoTerminar = -1; // DO NOT TOUCH
    public int progressoMinimo = -1;  // DO NOT TOUCH
    public int progressoMaximo = -1;  // DO NOT TOUCH
    public ConversaScriptable proximaConversa;
    public int id;

    private static Dictionary<int, bool> conversasExecutadas;

    public static int maiorId;

    static ConversaScriptable() {
        conversasExecutadas = new Dictionary<int, bool>();
    }

    public static void execute(int conversaId) {
        if (conversasExecutadas.ContainsKey(conversaId)) {
            conversasExecutadas[conversaId] = true;
        } else {
            conversasExecutadas.Add(conversaId, true);
        }

        if (conversaId > maiorId) {
            maiorId = conversaId;
        }
    }

    public static void setConversasExecutadas(Dictionary<int, bool> conversasExecutadas) {
        ConversaScriptable.conversasExecutadas = conversasExecutadas;
    }

    public static Dictionary<int, bool> getConversasExecutadas() {
        return conversasExecutadas;
    }

    public ConversaScriptable att() {
        bool executou;
        if (conversasExecutadas.TryGetValue(id, out executou)) {
            if (executou && proximaConversa != null) {
                return proximaConversa.att();
            }
        }
        return this;
    }

    public string getNome(int i) {
        return nomes[i].Replace("[Nickname]", PlayerStatus.nome); ;
    }

    public string getFala(int i) {
        return falas[i].Replace("[Nickname]", PlayerStatus.nome); ;
    }
}

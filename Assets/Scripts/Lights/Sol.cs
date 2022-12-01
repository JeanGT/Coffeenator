using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Sol : MonoBehaviour
{
    public const float duracaoMadrugada = 5; // Padrão: 5
    public const float duracaoDia = 200; // Padrão: 200
    public const float duracaoTardinha = 215; // Padrão: 215
    public Color luzDia; // Cor da luz durante o dia
    public Color luzTardinha; // Cor da luz durante a passagem do dia para a noite
    public Color luzNoite; // Cor da luz durante a noite
    public Color preto;

    public static float horarioDormir; //Horário em que é permitido dormir
    private UnityEngine.Rendering.Universal.Light2D thisLight; // Obrigatório ter no GameObject que tem o script

    void Start() {
        thisLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        horarioDormir = duracaoTardinha;
    }

    void Update() {
        if(PlayerStatus.horario < duracaoMadrugada) {
            float t = (duracaoMadrugada - PlayerStatus.horario) / duracaoMadrugada;
            thisLight.color = Color.Lerp(luzDia, preto, t);
        } else if (PlayerStatus.horario < duracaoDia) {
            float t = (duracaoDia - PlayerStatus.horario) / (duracaoDia - duracaoMadrugada);
            thisLight.color = Color.Lerp(luzTardinha, luzDia, t);
        } else if (PlayerStatus.horario < duracaoTardinha) { 
            float t = (duracaoTardinha - PlayerStatus.horario) / (duracaoTardinha - duracaoDia);
            thisLight.color = Color.Lerp(luzNoite, luzTardinha, t);
        } else {
            thisLight.color = luzNoite;
        }
    }

}

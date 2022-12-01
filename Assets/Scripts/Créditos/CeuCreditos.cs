using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeuCreditos : MonoBehaviour
{
    private float duracaoMadrugada;
    private float duracaoDia; // Tempo em segundos que dura o dia
    private float duracaoTardinha; // Tempo em segundos que dura a passagem do dia para a noite
    public Color luzDia; // Cor da luz durante o dia
    public Color luzTardinha; // Cor da luz durante a passagem do dia para a noite
    public Color luzNoite; // Cor da luz durante a noite

    private SpriteRenderer sp; // Obrigatório ter no GameObject que tem o script

    void Start()
    {
        CreditosManager sol = GameObject.Find("Main Camera").GetComponent<CreditosManager>();

        duracaoMadrugada = sol.duracaoMadrugada;
        duracaoDia = sol.duracaoDia;
        duracaoTardinha = sol.duracaoTardinha;
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (PlayerStatus.horario < duracaoMadrugada)
        {
            float t = (duracaoMadrugada - PlayerStatus.horario) / duracaoMadrugada;
            sp.color = Color.Lerp(luzDia, luzNoite, t);
        }
        else if (PlayerStatus.horario < duracaoDia)
        {
            float t = (duracaoDia - PlayerStatus.horario) / (duracaoDia - duracaoMadrugada);
            sp.color = Color.Lerp(luzTardinha, luzDia, t);
        }
        else if (PlayerStatus.horario < duracaoTardinha)
        {
            float t = (duracaoTardinha - PlayerStatus.horario) / (duracaoTardinha - duracaoDia);
            sp.color = Color.Lerp(luzNoite, luzTardinha, t);
        }
        else
        {
            sp.color = luzNoite;
        }
    }
}

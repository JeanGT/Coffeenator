using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MelhorarHabilidades : MonoBehaviour
{
    public float vidaStep;
    public float velocidadeAtaqueStep;
    public float velocidadeMovimentoStep;
    public float danoStep;

    public void melhorarVida()
    {
        PlayerStatus.setVida(PlayerStatus.getVida() + vidaStep);
        PlayerStatus.upsVida++;
        voltarAoDojo();
    }

    public void melhorarVelocidadeDeMovimento()
    {
        PlayerStatus.setVelocidade(PlayerStatus.getVelocidade() + velocidadeMovimentoStep);
        PlayerStatus.upsVelocidade++;
        voltarAoDojo();
    }

    public void melhorarVelocidadeDeAtaque()
    {
        PlayerStatus.setVelocidadeDeAtaque(PlayerStatus.getVelocidadeDeAtaque() + velocidadeAtaqueStep);
        PlayerStatus.upsVelocidadeAtaque++;
        voltarAoDojo();
    }

    public void melhorarDano()
    {
        PlayerStatus.setDano(PlayerStatus.getDano() + danoStep);
        PlayerStatus.upsDano++;
        voltarAoDojo();
    }

    private void voltarAoDojo()
    {
        GameManager.setPlayerX(PlayerStatus.getUltimoPlayerX());
        SceneManager.LoadScene(PlayerStatus.getUltimaCena(), LoadSceneMode.Single);
    }
}

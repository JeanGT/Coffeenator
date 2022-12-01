using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceberDinheiros : MonoBehaviour
{
    public int objetivoHackear = 8;
    public int objetivoEntregarSenha = 9;
    public int progressoAoHackear = 16;

    void Start()
    {
        if (PlayerStatus.getDinheirosAReceber() == float.MinValue) {
            PlayerStatus.fowardObjective(objetivoHackear, 1);
            PlayerStatus.addObjective(DicionarioObjetivos.getObjetivoByIndex(objetivoEntregarSenha));
            PlayerStatus.setProgresso(progressoAoHackear);
            PlayerStatus.setDinheirosAReceber(0);
        } else {
            if (PlayerStatus.getDinheirosAReceber() == 1 && PlayerStatus.getPaginasDesbloquadas() == 0)
            {
                GameManager.showMessage("Você concluiu todos os trabalhos até o momento. Você pode refazê-los quantas vezes quiser. Em breve surgirão novos.");
            }
            PlayerStatus.setDinheiros(PlayerStatus.getDinheiros() + PlayerStatus.getDinheirosAReceber());
            PlayerStatus.setDinheirosAReceber(0);
        }
    }
}

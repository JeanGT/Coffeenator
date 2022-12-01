using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaveMendigo : MonoBehaviour
{
    public Conversa conversaAposDerrotarMendigo;
    public Conversa conversaAntesDerrotarMendigo;
    public Conversa conversaProcurarChave;
    public static GameObject mendigo;
    public static bool derrotou;
    public static GameObject avisoMendigo;
    public float aliadoOffset = 0.3f;

    private GameObject nicole;
    private GameObject alex;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerStatus.getAvisoMendigo() == -1)
        {
            avisoMendigo = GameObject.Find("Aviso Mendigo");
            Destroy(avisoMendigo);
            Destroy(this.gameObject);
        }
        if (PlayerStatus.isChaveMendigo())
        {
            conversaProcurarChave.enabled = false;
            conversaAposDerrotarMendigo.enabled = false;
        }

        nicole = GameObject.Find("Nicole");
        alex = GameObject.Find("Alex");

        if (derrotou)
        {
            conversaAntesDerrotarMendigo.enabled = false;
            conversaAposDerrotarMendigo.enabled = true;
            
            Transform player = GameObject.Find("Player").transform;

            nicole.GetComponent<Aliado>().enabled = false;
            alex.GetComponent<Aliado>().enabled = false;

            nicole.transform.position = new Vector3(player.position.x + aliadoOffset, player.position.y, player.position.z);
            alex.transform.position = new Vector3(player.position.x + aliadoOffset * 2, -0.176f, player.position.z);
        }
        else
        {
            mendigo = GameObject.Find("Mendigo");
            avisoMendigo = GameObject.Find("Aviso Mendigo");
            if (PlayerStatus.getProgresso() > 12 && PlayerStatus.getProgresso() < 18)
            {
                mendigo.SetActive(false);
                avisoMendigo.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (PlayerStatus.getProgresso() > 12 && PlayerStatus.isChaveMendigo())
        {
            nicole.GetComponent<Aliado>().enabled = true;
            alex.GetComponent<Aliado>().enabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bob : Interacao
{
    GameObject canvas;
    public int progressoComprasDesabilitadas = 13;
    public int progressoReceberChave = 16;
    public Conversa conversaAposCompra;
    public Conversa conversaAposNaoCompra;
    public Conversa conversaSemDinheiro;
    public Conversa conversaEntregarChave;
    public Conversa conversaIntroducao;
    public float xOffset;
    public float velocidade;
    public Animator anim;
    private bool habilitado;
    private bool comecouAndar;
    private float initX;

    private const float precoProjetil1 = 3;
    private const float precoProjetil2 = 50;
    private const float precoProjetil3 = 1999.99f;

    private GameObject btnProjeteil1;
    private GameObject btnProjeteil2;
    private GameObject btnProjeteil3;

    // Start is called before the first frame update
    void Start()
    {
        StartObjeto();
        initX = transform.position.x;
        conversaAposCompra.enabled = false;
        conversaAposNaoCompra.enabled = false;
        conversaSemDinheiro.enabled = false;
        conversaIntroducao.enabled = false;
        conversaEntregarChave.enabled = false;

        canvas = GameObject.Find("Opcoes Bob");
        canvas.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateObjeto();
        if ((PlayerStatus.horario > Sol.duracaoTardinha || habilitado) && transform.position.x == initX) {
            habilitado = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        } else if (PlayerStatus.horario > Sol.duracaoDia) {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            if (!comecouAndar) {
                comecouAndar = true;
                transform.position += new Vector3(xOffset, 0, 0);
                anim.SetBool("andando", true);
            }
            transform.position -= new Vector3(velocidade * Time.deltaTime, 0, 0);
            if(transform.position.x < initX) {
                transform.position = new Vector3(initX, transform.position.y, transform.position.z);
                habilitado = true;
                anim.SetBool("andando", false);
            }
        } else {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public override void interagir() {
        canvas.SetActive(true);
        conversaIntroducao.enabled = true;

        btnProjeteil1 = GameObject.Find("Projetil 1");
        btnProjeteil2 = GameObject.Find("Projetil 2");
        btnProjeteil3 = GameObject.Find("Projetil 3");

        if (PlayerStatus.projetil > 0)
        {
            if (btnProjeteil1 != null)
            Destroy(btnProjeteil1);
            if (PlayerStatus.projetil > 1)
            {
                if (btnProjeteil2 != null)
                    Destroy(btnProjeteil2);
                if (PlayerStatus.projetil > 2)
                {
                    if (btnProjeteil3 != null)
                        Destroy(btnProjeteil3);
                }
            }
        }
    }

    private bool efetuarCompra(float valor, Conversa conversa) {
        if (PlayerStatus.getDinheiros() >= valor) {
            PlayerStatus.setDinheiros(PlayerStatus.getDinheiros() - valor);
            conversa.enabled = true;

            GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
            if (caixaDeSom != null)
            {
                caixaDeSom.GetComponent<MusicaDeFundo>().playSound(51);
            }

            return true;
        }
        conversaSemDinheiro.enabled = true;
        return false;
    }

    public void comprarProjetil1() {
        canvas.SetActive(false);
        bool comprou = efetuarCompra(precoProjetil1, conversaAposCompra);
        if (comprou) {
            PlayerStatus.projetil = 1;
        }
    }

    public void comprarProjetil2() {
        canvas.SetActive(false);
        bool comprou = efetuarCompra(precoProjetil2, conversaAposCompra);
        if (comprou) {
            PlayerStatus.projetil = 2;
        }
    }

    public void comprarProjetil3() {
        canvas.SetActive(false);
        bool comprou = efetuarCompra(precoProjetil3, conversaAposCompra);
        if (comprou) {
            PlayerStatus.projetil = 3;
        }
    }

    public void sair() {
        conversaAposNaoCompra.enabled = true;
        canvas.SetActive(false);
    }

    public void EntregarChave() {
        conversaEntregarChave.enabled = true;
    }
}

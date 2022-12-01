
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class OrochiTransformado : Inimigo
{
    public float paredeOffSet;

    public float timeWalking;
    public float timeStopped;
    public float timeBetweenParedes;
    public float nWaves;
    public float timeBetweenFogos;
    public float timeBetweenFogosSpawn;

    public float aumentoTimeBetweenFogos;
    private Transform outside;
    public Transform notReverse;
    private Transform paredeCima;
    private Transform paredeBaixo;
    private Transform paredeDireita;
    private Transform paredeEsquerda;
    public Transform[] projetilSpawns;
    private GameObject[] fogoSpawns;
    public ArrayList fogoSpawnsDisponiveis;
    private Transform centro;
    public GameObject parede;
    public GameObject projetil;
    public GameObject fogo;
    public UnityEngine.Rendering.Universal.Light2D sol;
    public Color corApagado;
    public Color corInicial;
    public float timeBetweenShots;
    public float velocidadeProjeteis;
    public float tempoAteReaparecer;
    public float tempoTudoEscuro;

    private float cTempoTudoEscuro;
    private GameObject kanjiI;
    private float cTimeBetweenFogos;
    private float cTimeBetweenFogosSpawn;
    private float cTempoAteReaparecer;
    private bool primeiroFrame = true;
    private GameObject[] projetilSpawnados;
    private Transform[] nextProjetilSpawns;
    private float cTimeBetweenShots;
    private float cTimeBetweenParedes;
    public GameObject kanji;

    private int cNWaves;
    private int cNProjeteis;
    private int cNFogos;
    private float cTimeWalking;
    private float cTimeStopped;
    private Vector2 walkDir;
    private bool ataqueNormal = true;

    private void resetarFogoSpawnsDisponiveis() {
        fogoSpawnsDisponiveis = new ArrayList();
        for(int i = 0; i < fogoSpawns.Length; i++) {
            fogoSpawnsDisponiveis.Add(fogoSpawns[i]);
        }
    }

    void Start()
    {
        paredeEsquerda = GameObject.Find("barreira").transform;
        paredeDireita = GameObject.Find("barreira (1)").transform;
        paredeBaixo = GameObject.Find("barreira (2)").transform;
        paredeCima = GameObject.Find("barreira (3)").transform;

        centro = GameObject.Find("Centro").transform;

        sol = GameObject.Find("Sol").GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        outside = GameObject.Find("Outside").transform;

        fogoSpawns = GameObject.FindGameObjectsWithTag("FogoSpawn");

        cTimeBetweenFogos = timeBetweenFogos;
        resetarFogoSpawnsDisponiveis();
        sol.color = corInicial;
        projetilSpawnados = new GameObject[projetilSpawns.Length];
        cVelocidade = velocidade;
        InimigoStart();
        setWalkDir();
    }

    private void FixedUpdate()
    {
        InimigoFixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        InimigoUpdate();
    }

    protected override void move()
    {
        if (!isFreezed && !isStunned)
        {
            if (cTimeWalking < timeWalking)
            {
                anim.SetBool("andando", true);
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + walkDir.normalized * cVelocidade * Time.fixedDeltaTime);
                cTimeWalking += Time.fixedDeltaTime;
            }
            else
            {
                if (cTimeStopped > timeStopped)
                {
                    attack();
                } else {
                    anim.SetBool("andando", false);
                    cTimeStopped += Time.fixedDeltaTime;
                }
            }
        }
    }

    private void attack()
    {
        if (ataqueNormal) {
            if (cNWaves < nWaves) {
                if (cTimeBetweenParedes > timeBetweenParedes) {
                    cTimeBetweenParedes = 0;
                    int spawn = Random.Range(0, 4);
                    Transform spawnT = null;
                    float rotation = 0;
                    Vector3 offSet = new Vector3();
                    switch (spawn) {
                        case 0:
                            offSet = new Vector3(0, -paredeOffSet, 0);
                            spawnT = paredeCima;
                            break;
                        case 1:
                            offSet = new Vector3(0, paredeOffSet, 0);
                            spawnT = paredeBaixo;
                            break;
                        case 2:
                            offSet = new Vector3(-paredeOffSet, 0, 0);
                            rotation = 90;
                            spawnT = paredeDireita;
                            break;
                        case 3:
                            offSet = new Vector3(paredeOffSet, 0, 0);
                            rotation = 90;
                            spawnT = paredeEsquerda;
                            break;
                    }
                    GameObject paredeI = Instantiate(parede, spawnT.position, Quaternion.identity);
                    paredeI.GetComponent<Projetil>().shooter = spawnT;
                    paredeI.transform.Rotate(0, 0, rotation);
                    paredeI.transform.position += offSet;
                } else {
                    cTimeBetweenParedes += Time.fixedDeltaTime;
                }
      
                if (cTimeBetweenShots > timeBetweenShots) {
                    cTimeBetweenShots = 0;
                    GameObject projetilI = Instantiate(projetil, projetilSpawns[cNProjeteis].position, Quaternion.identity);
                    projetilI.GetComponent<ProjetilOrochi>().shooter = transform;
                    projetilI.transform.parent = projetilSpawns[cNProjeteis];
                    projetilSpawnados[cNProjeteis] = projetilI;
                    cNProjeteis++;
                    if (cNProjeteis == projetilSpawns.Length) {
                        cNProjeteis = 0;
                        anim.SetBool("atacando", true);
                        
                        MusicaDeFundo caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
                        int som = Random.Range(21, 22);

                        if(caixaDeSom != null)
                        {
                            caixaDeSom.playSound(som);
                        }

                        for (int i = 0; i < projetilSpawns.Length; i++) {
                            projetilSpawnados[i].GetComponent<ProjetilOrochi>().velocidade = velocidadeProjeteis;
                        }
                        cNWaves++;
                    }
                } else {
                    anim.SetBool("atacando", false);
                    anim.SetBool("carregando", true);
                    cTimeBetweenShots += Time.fixedDeltaTime;
                }
            } else {
                anim.SetBool("carregando", false);
                anim.SetBool("atacando", true);
                anim.SetBool("andando", true);
                anim.SetBool("atacando", false);
                
                terminarAtaque();
            }
        } else {
            cTempoTudoEscuro += Time.fixedDeltaTime;
            if (cTempoTudoEscuro > tempoTudoEscuro) {
                if (primeiroFrame) {
                    primeiroFrame = false;
                    player.position = centro.position;
                    kanjiI = Instantiate(kanji, player);
                }

                if (fogoSpawnsDisponiveis.Count > 0) {
                    cTimeBetweenFogosSpawn += Time.fixedDeltaTime;
                    cTimeBetweenFogos += aumentoTimeBetweenFogos * Time.fixedDeltaTime;
                    if (cTimeBetweenFogosSpawn > timeBetweenFogosSpawn) {

                        MusicaDeFundo caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
                        int som = Random.Range(33, 37);

                        if (caixaDeSom != null)
                        {
                            caixaDeSom.playSound(som);
                        }

                        cTimeBetweenFogosSpawn = 0;
                        int aux = Random.Range(0, fogoSpawnsDisponiveis.Count);
                        GameObject fogoI = Instantiate(fogo, ((GameObject) fogoSpawnsDisponiveis[aux]).transform.position, Quaternion.identity);
                        fogoSpawnsDisponiveis.RemoveAt(aux);
                        fogoI.GetComponent<Corote>().shooter = player;
                        fogoI.GetComponent<Corote>().tempoParado = (fogoSpawns.Length * timeBetweenFogosSpawn) + cNFogos * cTimeBetweenFogos;
                        fogoI.GetComponent<Corote>().tempoAteDesaparecer += fogoI.GetComponent<Corote>().tempoParado;
                        cNFogos++;
                    }
                } else {
                    cTempoAteReaparecer += Time.fixedDeltaTime;
                    if (cTempoAteReaparecer > tempoAteReaparecer) {
                        if (fumacaI == null) {
                            jogarFumaca(centro.position);
                        } else if (fumacaI.GetComponent<Fumaca>().desapareceu) {
                            fumacaI.GetComponent<Fumaca>().podeDesaparecer = true;
                            transform.position = centro.position;
                            primeiroFrame = true;
                            cTempoTudoEscuro = 0;
                            cTempoAteReaparecer = 0;
                            cNFogos = 0;
                            Destroy(kanjiI);
                            cTimeBetweenFogos = timeBetweenFogos;
                            sol.color = corInicial;
                            resetarFogoSpawnsDisponiveis();
                            terminarAtaque();
                        }
                    }
                }
            } else {
                anim.SetBool("andando", true);
                transform.position = outside.position;
                sol.color = corApagado;
            }
        }
    }

    private void terminarAtaque() {
        cTimeWalking = 0;
        cTimeStopped = 0;
        cNWaves = 0;
        cNFogos = 0;
        ataqueNormal = !ataqueNormal;
        setWalkDir();
    }

    private void setWalkDir()
    {
        walkDir = (player.position - transform.position).normalized;
    }

    protected override void onReverseScale(bool initialScale) {
        if (initialScale) {
            notReverse.localScale = new Vector3(1, 1, 1);
        } else {
            notReverse.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected override void onDie() {}
}

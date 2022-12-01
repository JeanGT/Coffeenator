using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerProgramar : MonoBehaviour
{
    public ShakeBehaviour shakeScreen;

    public int HP;
    public Slider charge;
    public GameObject bulletPrefab;
    public GameObject bulletPrefabBurst;
    public GameObject bulletPrefabGrande;
    public GameObject bulletPrefabGrandeBurst;
    public GameObject ia2;
    public GameObject ia3;
    public float spawnDistance;
    public float bulletVelocity;
    public float timeBetweenShots;

    public ProgramarManager manager;
    
    public GameObject canhaoAssistenteCSprite;
    public GameObject canhaoAssistenteBSprite;
    public GameObject canhaoAssistenteDSprite;
    public Image[] hps;

    public Sprite coracaoVazio;

    public float timeSpecial = 4;
    public float specialReloadTime = 16;
    public Button especialButton;
    public Slider sliderEspecial;
    public GameObject indicadorEspecialPronto;
    public GameObject especialPronto;
    public GameObject txtEspecial;
    public GameObject firewal;
    public GameObject firewallProjetilPrefab;

    private float cTimeSpecial;

    private float cTimeBetweenShots;
    private float cSpecialReloadTime;
    private float timeSinceLastShot;
    private bool canhaoAssistenteC;
    private bool canhaoAssistenteB;

    private int cCanhao;

    private bool subindo = true;

    private Animator anim;
    private Animator animB;
    private Animator animC;
    private bool hack = true;
    private bool superIA = false; //Muda o especial
    private bool superBurst = false; //Muda o especial
    private bool balaMaior = false; //Muda o projétil
    private int multDano = 1; //Muda o especial

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        animB = canhaoAssistenteBSprite.GetComponent<Animator>();
        animC = canhaoAssistenteCSprite.GetComponent<Animator>();

        cSpecialReloadTime = specialReloadTime;

        timeSinceLastShot = 0;

        if(PlayerStatus.getProgresso() < 15)
        {
            hack = false;
            Destroy(firewal);
        }

        if (PlayerStatus.getInteligencia()[0])
        {
            timeBetweenShots /= 1.1f;
        }

        if (!PlayerStatus.getInteligencia()[9])
        {
            especialButton.interactable = false;
            sliderEspecial.value = 0;
        }

        if (PlayerStatus.getInteligencia()[1]) {
            canhaoAssistenteC = true;
            timeBetweenShots /= 2;
        }

        if (PlayerStatus.getInteligencia()[2]) {
            canhaoAssistenteB = true;
            timeBetweenShots /= 1.5f;
        }

        if (canhaoAssistenteC) {
            canhaoAssistenteCSprite.SetActive(true);
        }
        else
        {
            canhaoAssistenteCSprite.SetActive(false);
        }

        if (canhaoAssistenteB)
        {
            canhaoAssistenteBSprite.SetActive(true);
        }
        else
        {
            canhaoAssistenteBSprite.SetActive(false);
        }

        if (PlayerStatus.getInteligencia()[6])
        {
            timeBetweenShots /= 2.25f;
        } else if (PlayerStatus.getInteligencia()[5])
        {
            timeBetweenShots /= 1.5f;
        }

        if (PlayerStatus.getInteligencia()[8])
        {
            bulletVelocity *= 3;
            
        } else if (PlayerStatus.getInteligencia()[7]) {
            bulletVelocity *= 1.5f;
        }

        cTimeBetweenShots = timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        sliderEspecial.value = cSpecialReloadTime;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Time.timeScale != 0) {
            transform.position = new Vector3(transform.position.x, worldMousePos.y, transform.position.z);
        }

        if (timeSinceLastShot > cTimeBetweenShots) {
            timeSinceLastShot = 0;

            int canhao = Random.Range(44, 50);
            int teclado = Random.Range(67, 71);

            ProgramarManager.caixaDeSom.playSound(canhao);
            ProgramarManager.caixaDeSom.playSound(teclado);

            if (cTimeSpecial > 0)
            {
                GameObject bullet = instantiateBullet(this.transform.GetChild(0).gameObject);
                bullet.transform.right = Vector3.right;
                bullet.GetComponent<Rigidbody2D>().velocity = Vector3.right * bulletVelocity;
                anim.SetBool("atirando", true);

                bullet.GetComponent<ProjetilProgramar>().damage *= multDano;

                if (!canhaoAssistenteC && !canhaoAssistenteB)
                {
                    GameObject bulletD = instantiateBullet(canhaoAssistenteDSprite);
                    bulletD.transform.right = Vector3.right;
                    bulletD.GetComponent<Rigidbody2D>().velocity = Vector3.right * bulletVelocity;

                    bulletD.GetComponent<ProjetilProgramar>().damage *= multDano;

                    GameObject bulletC = instantiateBullet(canhaoAssistenteCSprite);
                    bulletC.transform.right = Vector3.right;
                    bulletC.GetComponent<Rigidbody2D>().velocity = Vector3.right * bulletVelocity;
                    animC.SetBool("atirando", true);

                    bulletC.GetComponent<ProjetilProgramar>().damage *= multDano;

                    GameObject bulletB = instantiateBullet(canhaoAssistenteBSprite);
                    bulletB.transform.right = Vector3.right;
                    bulletB.GetComponent<Rigidbody2D>().velocity = Vector3.right * bulletVelocity;
                    animB.SetBool("atirando", true);

                    bulletB.GetComponent<ProjetilProgramar>().damage *= multDano;
                }

                if (canhaoAssistenteC) {
                    GameObject bulletC = instantiateBullet(canhaoAssistenteCSprite);
                    bulletC.transform.right = Vector3.right;
                    bulletC.GetComponent<Rigidbody2D>().velocity = Vector3.right * bulletVelocity;
                    animC.SetBool("atirando", true);

                    bulletC.GetComponent<ProjetilProgramar>().damage *= multDano;
                }

                if (canhaoAssistenteB) {
                    GameObject bulletB = instantiateBullet(canhaoAssistenteBSprite);
                    bulletB.transform.right = Vector3.right;
                    bulletB.GetComponent<Rigidbody2D>().velocity = Vector3.right * bulletVelocity;
                    animB.SetBool("atirando", true);

                    bulletB.GetComponent<ProjetilProgramar>().damage *= multDano;
                }
            }
            else
            {
                switch (cCanhao)
                {
                    case 0:
                        GameObject bullet = instantiateBullet(this.transform.GetChild(0).gameObject);
                        bullet.transform.right = Vector3.right;
                        bullet.GetComponent<Rigidbody2D>().velocity = Vector3.right * bulletVelocity;
                        anim.SetBool("atirando", true);

                        if (canhaoAssistenteC)
                        {
                            cCanhao = 1;
                        }
                        if (!subindo)
                        {
                            cCanhao = 2;
                        }
                        break;
                    case 1:
                        GameObject bulletC = instantiateBullet(canhaoAssistenteCSprite);
                        bulletC.transform.right = Vector3.right;
                        bulletC.GetComponent<Rigidbody2D>().velocity = Vector3.right * bulletVelocity;
                        animC.SetBool("atirando", true);

                        if (canhaoAssistenteB)
                        {
                            subindo = false;
                        }
                        else
                        {
                            subindo = true;
                        }
                        cCanhao = 0;
                        break;
                    case 2:
                        GameObject bulletB = instantiateBullet(canhaoAssistenteBSprite);
                        bulletB.transform.right = Vector3.right;
                        bulletB.GetComponent<Rigidbody2D>().velocity = Vector3.right * bulletVelocity;
                        animB.SetBool("atirando", true);

                        cCanhao = 0;
                        subindo = true;
                        break;
                }
            }
        } else {

            if (!manager.getAcabou()) {
                timeSinceLastShot += Time.deltaTime;
            }

            charge.value = timeSinceLastShot / cTimeBetweenShots;
            anim.SetBool("atirando", false);
            animB.SetBool("atirando", false);
            animC.SetBool("atirando", false);
        }

        if (cTimeSpecial > 0) {
            cTimeSpecial -= Time.deltaTime;
        }
        else
        {
            cTimeBetweenShots = timeBetweenShots;
            ia2.SetActive(false);
            ia3.SetActive(false);
        }

        if (cSpecialReloadTime < specialReloadTime)
        {
            cSpecialReloadTime += Time.deltaTime;
        }
        else if (PlayerStatus.getInteligencia()[9])
        {
            if (!indicadorEspecialPronto.activeInHierarchy)
            {
                indicadorEspecialPronto.SetActive(true);
                txtEspecial.SetActive(true);
                especialPronto.SetActive(true);
            }
        }
    }

    private GameObject instantiateBullet(GameObject canhao)
    {
        if (balaMaior)
        {
            if (superBurst && cTimeSpecial > 0)
            {
                return (GameObject)Instantiate(bulletPrefabGrandeBurst, canhao.transform.position + new Vector3(spawnDistance, 0, 0), Quaternion.identity);
            }

            return (GameObject)Instantiate(bulletPrefabGrande, canhao.transform.position + new Vector3(spawnDistance, 0, 0), Quaternion.identity);
        }
        
        if (superBurst && cTimeSpecial > 0)
        {
            return (GameObject)Instantiate(bulletPrefabBurst, canhao.transform.position + new Vector3(spawnDistance, 0, 0), Quaternion.identity);
        }

        return (GameObject)Instantiate(bulletPrefab, canhao.transform.position + new Vector3(spawnDistance, 0, 0), Quaternion.identity);
    }

    public void hit(int damage) {
        HP -= damage;
        shakeScreen.TriggerShake();

        if (HP <= 0) {
            hps[0].sprite = coracaoVazio;
            hps[1].sprite = coracaoVazio;
            hps[2].sprite = coracaoVazio;
            die();
        } else if (HP == 1) {
            hps[1].sprite = coracaoVazio;
            hps[2].sprite = coracaoVazio;
        } else if (HP == 2) {
            hps[2].sprite = coracaoVazio;
        }

    }

    private void die() {
        manager.perder();
    }

    public void onBtnEspecial()
    {
        if (cSpecialReloadTime >= specialReloadTime)
        {
            cTimeSpecial = timeSpecial;
            cSpecialReloadTime = 0;

            indicadorEspecialPronto.SetActive(false);
            txtEspecial.SetActive(false);
            especialPronto.SetActive(false);

            MusicaDeFundo caixaDeSom = GameObject.Find("MusicaPlayer").GetComponent<MusicaDeFundo>();
            if (caixaDeSom != null)
                caixaDeSom.playSound(4);

            if (hack)
            {
                GameObject firewallProjetil = (GameObject)Instantiate(firewallProjetilPrefab, new Vector3(-10, -2.67f, 0), Quaternion.identity);
                firewallProjetil.transform.right = Vector3.right;
                firewallProjetil.GetComponent<Rigidbody2D>().velocity = Vector3.right * 8;

                if (multDano != 1) {
                    
                }

                if (superBurst)
                {
                    cTimeBetweenShots /= 3;
                }

                if (superIA)
                {
                    ia2.SetActive(true);
                    ia3.SetActive(true);
                }
            }
        }
    }

    public void setMultDano(int mult)
    {
        multDano = mult;
    }

    public void ativarSuperIA()
    {
        superIA = true;
    }

    public void ativarSuperBurst()
    {
        superBurst = true;
    }

    public void ativarProjetilMaior()
    {
        balaMaior = true;
    }
}

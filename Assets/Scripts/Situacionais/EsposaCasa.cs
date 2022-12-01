using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsposaCasa : MonoBehaviour
{
    public int progressoParaSumir = 4;
    public int progressoParaReaparecer = 20;
    public int progressoCutscene = 100;
    public float xOnReapear;

    private Transform player;
    private float initXScale;
    private bool fix = false;

    // Start is called before the first frame update
    void Start()
    {
        initXScale = transform.localScale.x;
        player = GameObject.Find("Player").transform;
        if(PlayerStatus.getProgresso() >= progressoParaSumir && PlayerStatus.getProgresso() < progressoParaReaparecer) {
            Destroy(this.gameObject);
        } else if (PlayerStatus.getProgresso() >= progressoParaReaparecer) {
            if (PlayerStatus.getProgresso() < progressoCutscene) {
                transform.position = new Vector3(xOnReapear, transform.position.y, transform.position.z);
            }
        }
    }

    void Update() {
        if(player.position.x > transform.position.x) {
            transform.localScale = new Vector3(initXScale, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(-initXScale, transform.localScale.y, transform.localScale.z);
        }

        if (PlayerStatus.getProgresso() == 24) {
            if (!fix)
            {
                fix = true;
                GetComponent<AudioSource>().Play();

                GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
                if (caixaDeSom != null)
                {
                    caixaDeSom.GetComponent<MusicaDeFundo>().pauseBg();
                }
            }
        }
    }
}

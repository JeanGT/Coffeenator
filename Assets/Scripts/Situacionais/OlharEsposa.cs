using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlharEsposa : MonoBehaviour
{
    public CameraController cam;
    public GameObject esposa;
    public AudioClip musicaReencontro;

    void Start()
    {
        if (PlayerStatus.getProgresso() == 23)
        {
            GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
            if (caixaDeSom != null)
            {
                caixaDeSom.GetComponent<MusicaDeFundo>().setBackground(musicaReencontro);
                caixaDeSom.GetComponent<MusicaDeFundo>().playBg();
            }

            StartCoroutine(olharEsposa());
        }
    }

    IEnumerator olharEsposa()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().setFreeze(true);
        cam.player = esposa.transform;

        yield return new WaitForSeconds(2);

        cam.player = player.transform;
        player.GetComponent<Player>().setFreeze(false);
    }
}

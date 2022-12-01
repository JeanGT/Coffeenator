using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Escolta : MonoBehaviour
{
    public int progressoAparecer = 21;
    public float lojaX;
    public Transform playerTransform;
    public Player player;
    public Animator playerAnim;

    private Animator anim;
    private bool played;

    void Start()
    {
        anim = GetComponent<Animator>();
        if(PlayerStatus.getProgresso() < progressoAparecer) {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        if(PlayerStatus.getProgresso() == progressoAparecer + 1) {
            if (transform.position.x < lojaX) {
                if (playerTransform.position.x > lojaX)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    player.setFreeze(true);
                    playerAnim.SetBool("movendo", true);
                    playerTransform.position -= new Vector3(Time.fixedDeltaTime * player.velocidade, 0, 0);
                    player.lookLeft();
                    anim.SetBool("correndo", false);
                }
                GameManager.setPlayerX(-1.5f);
                GameManager.setPlayerOlhandoEsquerda(false);
                SceneManager.LoadScene("OrochiTransformação", LoadSceneMode.Single);
            } else {
                transform.localScale = new Vector3(-1, 1, 1);
                player.setFreeze(true);
                playerAnim.SetBool("movendo", true);
                playerTransform.position -= new Vector3(Time.fixedDeltaTime * player.velocidade, 0, 0);
                transform.position -= new Vector3(Time.fixedDeltaTime * player.velocidade, 0, 0);
                player.lookLeft();
                anim.SetBool("correndo", true);
            }
        }
    }
}

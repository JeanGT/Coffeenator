using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
	public Transform player;
	public float quantidadeSuavizacao;
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;
    public float playerYOffset = 5;

    public int cutsceneProgressTrigger = -2;
    public float cutsceneTargetX;

    public float duracaoCutscene;
    public float tempoParadoCutscene;
    public AudioClip musicaTensa;

    private float cDuracaoCutscene;
    private float cutsceneInitialX;
	private float suavizacaoSpeedX;
	private float suavizacaoSpeedY;

    private float width;
    private float height;

    private bool onCutscene;

    private void Start() {
        Vector3 size = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        width = size.x - transform.position.x;
        height = size.y - transform.position.y;

        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        corrigirPosicao();

        if (PlayerStatus.getProgresso() == cutsceneProgressTrigger) {
            cutsceneInitialX = transform.position.x;
            player.GetComponent<Player>().setFreeze(true);
            onCutscene = true;

            GameObject caixaDeSom = GameObject.Find("MusicaPlayer");
            if (caixaDeSom != null)
            {
                caixaDeSom.GetComponent<MusicaDeFundo>().setBackground(musicaTensa);
                caixaDeSom.GetComponent<MusicaDeFundo>().playBg();
            }
        }
    }

    private void goTo(float targetX) {
        transform.position = new Vector3(cutsceneInitialX + (targetX - cutsceneInitialX) * Mathf.Sin(cDuracaoCutscene / duracaoCutscene), transform.position.y, transform.position.z);
        cDuracaoCutscene += Time.fixedDeltaTime;
        if (cDuracaoCutscene > duracaoCutscene + tempoParadoCutscene) {
            onCutscene = false;
            cDuracaoCutscene = 0;
            cutsceneTargetX = cutsceneInitialX;
            cutsceneInitialX = transform.position.x;
            player.GetComponent<Player>().setFreeze(false);
        }
    }

    void FixedUpdate ()
	{
		seguirPlayer ();
		corrigirPosicao();

        if (onCutscene) {
            goTo(cutsceneTargetX);
        }
	}

	private void seguirPlayer ()
	{
		this.transform.position = new Vector3 (Mathf.SmoothDamp (this.transform.position.x, player.position.x, ref suavizacaoSpeedX, quantidadeSuavizacao), player.position.y + playerYOffset, this.transform.position.z); //segue o player com suavizacao
	}

	private void corrigirPosicao ()
	{
		if (transform.position.y - height < minY) {
			transform.position = new Vector3 (transform.position.x, minY + height, transform.position.z);
		} else if (transform.position.y + height > maxY) {
			transform.position = new Vector3 (transform.position.x, maxY - height, transform.position.z);
		}

		if (transform.position.x - width < minX) {
			transform.position = new Vector3 (minX + width, transform.position.y, transform.position.z);
		} else if (transform.position.x + width> maxX) {
			transform.position = new Vector3 (maxX - width, transform.position.y, transform.position.z);
		}

	}
}

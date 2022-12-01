using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DiminuirIntensidadeTocha : MonoBehaviour
{
    UnityEngine.Rendering.Universal.Light2D luz;
    public float velocidade;

    void Start()
    {
        luz = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    void Update()
    {
        if (PlayerStatus.getProgresso() == 27) {
            if (GetComponent<FireFlick>() != null) {
                Destroy(GetComponent<FireFlick>());
            }
            luz.intensity -= velocidade * Time.deltaTime;
            if(luz.intensity <= 0) {
                GameManager.setPlayerX(-0.41f);
                GameManager.setPlayerOlhandoEsquerda(false);
                SceneManager.LoadScene("Final", LoadSceneMode.Single);
            }
        }
    }
}

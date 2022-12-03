using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInicial : MonoBehaviour
{
    public GameObject avancarBtn;
    public GameObject[] dicas;

    public float tempoAteProximaDica;
    public int progressoMin = -1;
    public bool usaProgresso = false;

    private int progressoTutorial = -1;

    void Start()
    {
        Time.timeScale = 0f;

        if (PlayerStatus.getProgresso() > progressoMin && usaProgresso)
        {
            Time.timeScale = 1f;
            Destroy(this.transform.parent.gameObject);
        }
    }

    void Update()
    {
        if (progressoTutorial >= dicas.Length)
        {
            pularTutorial();
        }
    }

    public void avancarTutorial()
    {
        if(progressoTutorial >= 0)
            dicas[progressoTutorial].SetActive(false);

        progressoTutorial++;

        if (progressoTutorial < dicas.Length)
        {
            avancarBtn.SetActive(false);
            if(dicas[progressoTutorial] != null){
                dicas[progressoTutorial].SetActive(true);
            }

            StartCoroutine(esperarProximaDica());
        }
    }

    IEnumerator esperarProximaDica()
    {
        yield return new WaitForSecondsRealtime(tempoAteProximaDica);
        avancarBtn.SetActive(true);
    }

    public void pularTutorial()
    {
        if (usaProgresso)
        {
            Time.timeScale = 1f;
            PlayerStatus.setProgresso(progressoMin + 1);
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            Time.timeScale = 1f;
            Destroy(this.transform.parent.gameObject);
        }
    }
}

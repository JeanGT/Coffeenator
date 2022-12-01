using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;

    private bool desabilitou;


    private void LateUpdate() {
        if (!desabilitou) {
            gameObject.SetActive(false);
            desabilitou = true;
        }
    }

    public void resume() {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void pause() {
        if (!isPaused) {
            Time.timeScale = 0f;
            isPaused = true;
        } else {
            resume();
        }
    }

    public void voltarAoMenu() {
        resume();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}

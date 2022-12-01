using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayCutscene : MonoBehaviour
{
    public int progresso;
    public VideoClip cutscene;

    private void Start() {
        if(PlayerStatus.getProgresso() == progresso) {
            GameManager.playCutscene(cutscene);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueBoxComp : MonoBehaviour, IPointerDownHandler // required interface when using the OnPointerDown method.
 {
    private const float BUG_FIX_DELAY = 0.2f;
    private float timeActive = 0;

    void Update() {
        timeActive += Time.deltaTime;
        if (timeActive > BUG_FIX_DELAY) {
            if (GetComponent<IncreaseScale>().isInitialScale()) {
                gameObject.SetActive(false);
                transform.Find("dialogueTxt").GetComponent<Text>().text = "";
                GameObject.Find("Player").GetComponent<Player>().setFreeze(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (timeActive > BUG_FIX_DELAY) {
            GetComponent<IncreaseScale>().goBack();
        }
    }

    void OnEnable() {
        timeActive = 0;
    }
}

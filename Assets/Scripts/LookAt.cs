using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private float initXScale;
    private static string targetName = "Player";
    private static Transform target;

    void Start()
    {
        target = GameObject.Find(targetName).transform;
        initXScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.transform.position.x > transform.position.x) {
            transform.localScale = new Vector3(initXScale, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(-initXScale, transform.localScale.y, transform.localScale.z);
        }
    }

    public static void setTargetName(string targetName) {
        LookAt.targetName = targetName;
        target = GameObject.Find(targetName).transform;
    }
}

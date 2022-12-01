using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float minX;
    public float maxX;

    public float velocidade;

    public float aleatoridade;

    public float timeWalking;
    public float timeStopped;

    public float minDist;

    private float initialTimeWalking;
    private float intialTimeStopped;

    private float cTimeWalking;
    private float cTimeStopped;

    private float dir;
    private float initalScale = 1;

    private bool isInFrontOfDoor;

    public Animator animator;

    public bool parar;

    void Start()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        initialTimeWalking = timeWalking;
        intialTimeStopped = timeStopped;

        timeWalking = initialTimeWalking + Random.Range(-aleatoridade, aleatoridade);
        timeStopped = intialTimeStopped + Random.Range(-aleatoridade, aleatoridade);
        escolherDir();
    }

    // Update is called once per frame
    void Update()
    {

        if (cTimeStopped > timeStopped && !parar)
        {
            animator.SetBool("movendo", true);

            if (dir == 1)
            {
                lookRight();
            }
            else
            {
                lookLeft();
            }

            transform.position += new Vector3(velocidade * dir * Time.deltaTime, 0, 0);

            cTimeWalking += Time.fixedDeltaTime;
            if (cTimeWalking > timeWalking && !isInFrontOfDoor)
            {
                timeWalking = initialTimeWalking + Random.Range(-aleatoridade, aleatoridade);
                timeStopped = intialTimeStopped + Random.Range(-aleatoridade, aleatoridade);
                escolherDir();
                cTimeWalking = 0;
                cTimeStopped = 0;
            }
        }
        else
        {
            cTimeStopped += Time.deltaTime;
            animator.SetBool("movendo", false);
        }
    }

    public void lookRight()
    {
        transform.localScale = new Vector3(initalScale, transform.localScale.y, transform.localScale.z);
    }

    public void lookLeft()
    {
        transform.localScale = new Vector3(-initalScale, transform.localScale.y, transform.localScale.z);
    }

    private void escolherDir()
    {
        float distancia = velocidade * timeWalking;
        if (transform.position.x - distancia < minX)
        {
            dir = 1;
        }
        else if (transform.position.x + distancia > maxX)
        {
            dir = -1;
        }
        else if (Random.Range(0f, 1f) < 0.5f)
        {
            dir = 1;
        }
        else
        {
            dir = -1f;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        isInFrontOfDoor = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isInFrontOfDoor = false;
    }
}

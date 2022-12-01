using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehaviour : MonoBehaviour
{
    // Transform of the GameObject you want to shake
    private Transform transformShake;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.2f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1f;

    public Transform player;

    public bool isProgramar;

    void Awake()
    {
        if (transformShake == null)
        {
            transformShake = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            if (!isProgramar)
            {
                transformShake.position = new Vector3(player.position.x, player.position.y, transformShake.position.z) + Random.insideUnitSphere * shakeMagnitude;
            } else
            {
                transformShake.position = new Vector3(0, 0, -10) + Random.insideUnitSphere * shakeMagnitude;
            }

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    public void TriggerShake()
    {
        shakeDuration = 0.2f;
    }
}

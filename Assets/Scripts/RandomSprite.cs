using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
	public float frameRate;
	public Sprite[] sprites;

	private SpriteRenderer spriteRenderer;
	private float timeSinceLastChange;

	void Awake ()
	{
		spriteRenderer = GetComponent <SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		timeSinceLastChange += Time.deltaTime;
		if (timeSinceLastChange > frameRate) {
			spriteRenderer.sprite = sprites [Random.Range (0, sprites.Length)];
			timeSinceLastChange = 0;
		}
	}
}

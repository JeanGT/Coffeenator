using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOffset : MonoBehaviour {

    public Transform player;
    private Material cMaterial;
    private float offSet;
    private float playerInitialX;
    public float speed;

    private void Start() {
        cMaterial = GetComponent<Renderer>().material;
        playerInitialX = player.position.x;
    }

    // Update is called once per frame
    void Update () {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        offSet = player.position.x - playerInitialX;

        cMaterial.SetTextureOffset("_MainTex", new Vector2(offSet * speed, 0));
	}
}

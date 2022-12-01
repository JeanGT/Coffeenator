using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public GameObject[] npcs;
    public GameObject[] npcsDependentes;

    public float minX;
    public float maxX;

    private static GameObject[] npcsAtuais;
    public static bool npcsDia;

    private int npcsSpawnados;

    // Start is called before the first frame update
    void Start()
    {
        attNPCS();
        
        for(int i = 0; i < npcsAtuais.Length; i++) {
            spawnNPC(npcsAtuais[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void attNPCS() {
        float horario = PlayerStatus.horario;

        if(horario < Sol.duracaoDia && !npcsDia) {
            npcsAtuais = new GameObject[3];

            ArrayList npcsAux = new ArrayList();
            for(int i = 0; i < npcs.Length; i++) {
                npcsAux.Add(npcs[i]);
            }

            int indiceNPC1 = Random.Range(0, npcs.Length);
            int indiceNPC2 = Random.Range(0, npcs.Length - 1);

            GameObject npc1 = ((GameObject) npcsAux[indiceNPC1]);
            npcsAux.RemoveAt(indiceNPC1);
            GameObject npc2 = ((GameObject) npcsAux[indiceNPC2]);
            GameObject npc3 = npcsDependentes[Random.Range(0, npcsDependentes.Length)];

            npcsAtuais[0] = npc1;
            npcsAtuais[1] = npc2;
            npcsAtuais[2] = npc3;

            npcsDia = true;
        } else if (horario > Sol.duracaoDia && npcsDia) {
            npcsAtuais = new GameObject[3];

            ArrayList npcsAux = new ArrayList();
            for (int i = 0; i < npcsDependentes.Length; i++) {
                npcsAux.Add(npcsDependentes[i]);
            }

            int indiceNPC1 = Random.Range(0, npcsDependentes.Length);
            int indiceNPC2 = Random.Range(0, npcsDependentes.Length - 1);

            GameObject npc1 = ((GameObject)npcsAux[indiceNPC1]);
            npcsAux.RemoveAt(indiceNPC1);
            GameObject npc2 = ((GameObject)npcsAux[indiceNPC2]);
            GameObject npc3 = npcs[Random.Range(0, npcs.Length)];

            npcsAtuais[0] = npc1;
            npcsAtuais[1] = npc2;
            npcsAtuais[2] = npc3;

            npcsDia = false;
        }
    }

    private void spawnNPC(GameObject npc) {
        GameObject npcI = Instantiate(npc, new Vector3(Random.Range(minX, maxX), transform.position.y, 0), Quaternion.identity);

        if (Random.Range(0f, 1f) < 0.5f) {
            npcI.GetComponent<NPC>().lookRight();
        } else {
            npcI.GetComponent<NPC>().lookLeft();
        }

        SpriteRenderer s = npcI.GetComponent<SpriteRenderer>();
        if(s != null)
        s.sortingOrder = -3 - npcsSpawnados;
        npcsSpawnados++;
    }
}

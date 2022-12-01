using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBug : MonoBehaviour {
    public float minTimeBetweenSpawns;
    public float maxTimeBetweenSpawns;
    public float yRange;
    public GameObject[] inimigos;
    public Text[] txtInimigos;

    private float timetoNextSpawn = 0;
    private ArrayList lastBugs;
    private ArrayList nextBugs;
    private Dictionary<string, int> inimigosT;
    private Dictionary<string, int> inimigosw2;
    private Dictionary<string, int> inimigosw3;
    private float velocidadeSpawn;
    private bool primeira = true;
    private int wave;

    private void Start() {
        Vector3 size = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        transform.position = new Vector3(size.x, transform.position.y, transform.position.z);

        lastBugs = new ArrayList();
        nextBugs = new ArrayList();

        if (ProgramarManager.hackear) {
            inimigosT = DicionarioBugs.getInimigosByIndex(12);
            inimigosw2 = DicionarioBugs.getInimigosByIndex(13);
            inimigosw3 = DicionarioBugs.getInimigosByIndex(14);
            velocidadeSpawn = 0.8f;
        } else {
            inimigosT = DicionarioBugs.getInimigosByIndex(GameManager.getProximoTrabalho().inimigos);
            velocidadeSpawn = GameManager.getProximoTrabalho().velocidade;
        }

        foreach (KeyValuePair<string, int> x in inimigosT) {
            for (int i = 0; i < inimigos.Length; i++) {
                if (inimigos[i].name.Equals(x.Key)) {
                    for (int j = 0; j < x.Value; j++) {
                        nextBugs.Add(inimigos[i]);
                    }
                }
            }
        }

        attTxtInimigos();
    }

    public bool avancarWave()
    {
        if (wave == 0)
        {
            foreach (KeyValuePair<string, int> x in inimigosw2)
            {
                for (int i = 0; i < inimigos.Length; i++)
                {
                    if (inimigos[i].name.Equals(x.Key))
                    {
                        for (int j = 0; j < x.Value; j++)
                        {
                            nextBugs.Add(inimigos[i]);
                        }
                    }
                }
            }
            wave++;
            primeira = true;
            return false;

        } else if(wave == 1)
        {
            foreach (KeyValuePair<string, int> x in inimigosw3)
            {
                for (int i = 0; i < inimigos.Length; i++)
                {
                    if (inimigos[i].name.Equals(x.Key))
                    {
                        for (int j = 0; j < x.Value; j++)
                        {
                            nextBugs.Add(inimigos[i]);
                        }
                    }
                }
            }
            wave++;
            primeira = true;
            return false;

        } else
        {
            return true;
        }
    }

    public int getWave()
    {
        return wave;
    }

    private void attTxtInimigos() {
        int zigzag = 0, rapidinho = 0, glitch = 0, summoner = 0, shooter = 0;
        for (int i = 0; i < nextBugs.Count; i++) {
            string name = ((GameObject)nextBugs[i]).name;
            if (name == DicionarioBugs.ZIGUE_ZAGUE || name == DicionarioBugs.ZIGUE_ZAGUE_2) {
                zigzag++;
            } else if (name == DicionarioBugs.RAPIDINHO || name == DicionarioBugs.RAPIDINHO_2) {
                rapidinho++;
            } else if (name == DicionarioBugs.GLITCH || name == DicionarioBugs.GLITCH_2) {
                glitch++;
            } else if (name == DicionarioBugs.SUMMONER || name == DicionarioBugs.SUMMONER_2 || name == DicionarioBugs.SUMMONER_3 || name == DicionarioBugs.SUMMONER_E2) {
                summoner++;
            } else if (name == DicionarioBugs.SHOOTER || name == DicionarioBugs.SHOOTER_2 || name == DicionarioBugs.SHOOTER_3 || name == DicionarioBugs.SHOOTER_4 || name == DicionarioBugs.SHOOTER_5) {
                shooter++;
            }
        }

        txtInimigos[0].text = zigzag.ToString();
        txtInimigos[1].text = rapidinho.ToString();
        txtInimigos[2].text = glitch.ToString();
        txtInimigos[3].text = summoner.ToString();
        txtInimigos[4].text = shooter.ToString();
    }

    void Update() {
        if (timetoNextSpawn < 0) {
            if (nextBugs.Count > 0) {
                timetoNextSpawn = UnityEngine.Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns) * velocidadeSpawn;
                int x;
                if (primeira)
                {
                    x = 0;
                    primeira = false;
                } else {
                    x = UnityEngine.Random.Range(0, nextBugs.Count);
                }
                GameObject prefab = (GameObject)nextBugs[x];
                GameObject bug;

                if(prefab.GetComponent<Glitch>() != null)
                {
                    if (prefab.GetComponent<Glitch>().boss)
                    {
                        bug = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
                        lastBugs.Add(bug);
                    }
                }

                if (prefab.GetComponent<Glitch>() == null || !(prefab.GetComponent<Glitch>().boss))
                {
                    bug = (GameObject)Instantiate(prefab, transform.position + new Vector3(0, UnityEngine.Random.Range(-yRange, yRange), 0), Quaternion.identity);
                    lastBugs.Add(bug);
                }

                nextBugs.Remove(prefab);
                attBugsIndex();
                attTxtInimigos();
            }
        } else {
            timetoNextSpawn -= Time.deltaTime;
        }

    }

    public void removeBug(int index) {
        if (index < lastBugs.Count) {
            lastBugs.RemoveAt(index);
        }
        attBugsIndex();
    }

    public void addBug(GameObject bug) {
        lastBugs.Add(bug);
        attBugsIndex();
    }

    public ArrayList getNextBugs() {
        return this.nextBugs;
    }

    public void clearNextBugs()
    {
        nextBugs.Clear();
        attTxtInimigos();
    }

    public ArrayList getLastBugs() {
        return this.lastBugs;
    }

    private void attBugsIndex() {
        for (int i = 0; i < lastBugs.Count; i++) {
            ((GameObject)lastBugs[i]).GetComponent<Bug>().setIndex(i);
        }
    }

    public GameObject getLastBug() {
        if (lastBugs.Count > 0) {
            return (GameObject)lastBugs[0];
        }
        return null;
    }

    public GameObject getRandomBug()
    {
        if (lastBugs.Count > 0)
        {
            return (GameObject)lastBugs[UnityEngine.Random.Range(0, lastBugs.Count)];
        }
        return null;
    }
}

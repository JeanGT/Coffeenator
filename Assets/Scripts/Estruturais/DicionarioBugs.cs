using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DicionarioBugs
{
    private static ArrayList bugs = new ArrayList();

    public const string ZIGUE_ZAGUE = "Zigue-Zague";
    public const string ZIGUE_ZAGUE_2 = "Zigue-Zague 2";
    public const string ZIGUE_ZAGUE_3 = "Zigue-Zague 3";

    public const string RAPIDINHO = "Rapidinho";
    public const string RAPIDINHO_2 = "Rapidinho 2";

    public const string GLITCH = "Glitch";
    public const string GLITCH_2 = "Glitch 2";

    public const string SUMMONER = "Summoner";
    public const string SUMMONER_2 = "Summoner 2";
    public const string SUMMONER_3 = "Summoner 3";
    public const string SUMMONER_E2 = "SummonerE 2";

    public const string SHOOTER = "Shooter";
    public const string SHOOTER_2 = "Shooter 2";
    public const string SHOOTER_3 = "Shooter 3";
    public const string SHOOTER_4 = "Shooter 4";
    public const string SHOOTER_5 = "Shooter 5";

    static DicionarioBugs(){
        //java, python, javascript, salario, velociade, inimigos[]
        bugs.Add(new Dictionary<string, int>() { { ZIGUE_ZAGUE, 2 }, { RAPIDINHO, 1 } }); //0
        bugs.Add(new Dictionary<string, int>() { { ZIGUE_ZAGUE, 3 }, { RAPIDINHO, 3 } }); //1
        bugs.Add(new Dictionary<string, int>() { { ZIGUE_ZAGUE, 3 }, { RAPIDINHO, 5 }, { GLITCH, 7 }}); //2
        bugs.Add(new Dictionary<string, int>() { { SUMMONER, 1 }, { RAPIDINHO, 6 }}); //3

        bugs.Add(new Dictionary<string, int>() { { ZIGUE_ZAGUE, 8 }, { RAPIDINHO, 8 }, { SUMMONER, 1 } }); //4
        bugs.Add(new Dictionary<string, int>() { { ZIGUE_ZAGUE_2, 15 }, { RAPIDINHO_2, 12 } }); //5
        bugs.Add(new Dictionary<string, int>() { { SUMMONER, 1 }, { ZIGUE_ZAGUE_2, 12 }, { RAPIDINHO_2, 10 } }); //6
        bugs.Add(new Dictionary<string, int>() { { SHOOTER, 3 }, { GLITCH, 5}, { ZIGUE_ZAGUE, 8 } }); //7

        bugs.Add(new Dictionary<string, int>() { { SUMMONER_E2, 14 } }); //8
        bugs.Add(new Dictionary<string, int>() { { SHOOTER, 1 }, { SUMMONER_E2, 5 }, { SUMMONER, 2 }, { ZIGUE_ZAGUE_2, 20 }, { RAPIDINHO_2, 10 }, { GLITCH, 10 } }); //9
        bugs.Add(new Dictionary<string, int>() { { SHOOTER, 3 }, { SUMMONER, 3 }, { SUMMONER_E2, 8 } }); //10
        bugs.Add(new Dictionary<string, int>() { { SUMMONER_2, 1 }, { SHOOTER, 4 }, { RAPIDINHO_2, 15 } }); //11

        bugs.Add(new Dictionary<string, int>() { { SHOOTER_2, 1 }, { ZIGUE_ZAGUE_2, 15 }, { RAPIDINHO_2, 10 } }); //Wave 1
        bugs.Add(new Dictionary<string, int>() { { SHOOTER_3, 1 }, { GLITCH_2, 8}, { RAPIDINHO, 20 }, { RAPIDINHO_2, 15 }, { ZIGUE_ZAGUE_2, 30 } }); //Wave 2
        bugs.Add(new Dictionary<string, int>() { { SHOOTER_4, 1 }, { SHOOTER_5, 4 }, { RAPIDINHO, 25 }, { RAPIDINHO_2, 10 }, { GLITCH_2, 5 }, { ZIGUE_ZAGUE_2, 20 }, { ZIGUE_ZAGUE, 25 } }); //Wave 3
    }

    public static Dictionary<string, int> getInimigosByIndex(int index) {
        return (Dictionary<string, int>) bugs[index];
    }

    public static int getIndexByInimigos(Dictionary<string, int> inimigo) {
        return bugs.IndexOf(inimigo);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "spawnScriptable", menuName = "ScriptableObjects/spawnScriptable", order = 1)]
public class SpawnScriptable : ScriptableObject

{
    public int troopInit;
    public int troopMax;
    public whosSpawn who;
}


public enum whosSpawn
{
    neutral = 0,
    player = 1,
    ennemy = 2,
}
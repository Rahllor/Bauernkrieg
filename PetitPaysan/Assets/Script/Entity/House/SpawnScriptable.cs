using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "spawnScriptable", menuName = "ScriptableObjects/spawnScriptable", order = 1)]
public class SpawnScriptable : ScriptableObject
{
    public int troopInit;
    public int troopMax;
    public Owner who;
}


public enum Owner
{
    Unknown = -1,
    Neutral = 0,
    Player = 1,
    Enemy = 2,
}
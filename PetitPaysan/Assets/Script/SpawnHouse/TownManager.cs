using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    public SpawnScriptable dataSpawn;
    float timer = 0.0f;
    private int currentTroop;
    private whosSpawn currentOwner;
    void Start()
    {
        currentOwner = dataSpawn.who;
        currentTroop = dataSpawn.troopInit;
    }

    void Update()
    {
        generateTroop();
        Debug.Log(currentTroop);
    }

    private void generateTroop()
    {
        if (dataSpawn.who == 0)
            return;
        if (currentTroop < dataSpawn.troopMax)
        {
            if (timer >= 4f)
            {
                currentTroop++;
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void departTroop()
    {
        int troopSend = currentTroop -1;
        Debug.Log("Je fais spawner" + troopSend + " troupes");
        currentTroop -= troopSend;
    }

    public void changeOwner(whosSpawn newOwner)
    {
        currentOwner = newOwner;
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IA : MonoBehaviour
{


    public List<TownManager> townsOnMap = null;

    //private TownManager selectedPlayerTown = null;
    //private TownManager selectedTargetTown = null;

    private int countMaxUnits = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Agregate all enemy houses on the map
        GameObject[] tmpHouses = GameObject.FindGameObjectsWithTag("House");
        townsOnMap.Clear();
        foreach (GameObject el in tmpHouses)
        {
            TownManager town = el.GetComponent<TownManager>();

            if (town != null)
            {
                if (town.CurrentOwner == Owner.Enemy)
                {
                    townsOnMap.Add(town);
                }
            }
        }










        //Count all units in the map
        int tmpCountMaxUnits = 0;
        foreach(TownManager town in townsOnMap)
        {
            tmpCountMaxUnits += town.CurrentTroop;
        }
        
        countMaxUnits = tmpCountMaxUnits;



        foreach(TownManager town in townsOnMap)
        {
            if(town.CurrentTroop > 5)
            {
                this.CheckIfAttack(town);
            }
        }
        
        
    }

    private void CheckIfAttack(TownManager town)
    {

        List<GameObject> tmpHouses = new List<GameObject>();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("House");

        //Debug.Log(gos.Length);

        gos = gos.OrderBy((el) => (el.transform.position - town.transform.position).sqrMagnitude).ToArray();

        foreach(GameObject go in gos)
        {
            if(go.GetComponent<TownManager>().CurrentOwner != Owner.Enemy)
            {
                tmpHouses.Add(go);
            }
        }

        TownManager target = tmpHouses[0].GetComponent<TownManager>();
        if (target.CurrentTroop < (town.CurrentTroop - 1) )
        {
            Debug.Log("Enemy Launch Attack");
            target.HasAttacked = true;

            town.spawnTroop(target);
        }



    }
}

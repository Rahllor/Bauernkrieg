using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }


    private TownManager selectedPlayerTown = null;
    private TownManager selectedTargetTown = null;

    public List<TownManager> townsOnMap = null;

    #region variablesSliderComparison

    private int nbDeathPlayer = 0;
    private int nbDeathEnemy = 0;

    public Slider sliderComparison;
    public Text deathPlayerText;
    public Text deathEnemyText;

    private int pondPlayer = 0;
    private int pondEnemy = 0;

    #endregion

    public Canvas LoseCanvas;
    public Canvas WinCanvas;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        //Agregate all houses on the map
        GameObject[] tmpHouses = GameObject.FindGameObjectsWithTag("House");
        
        foreach(GameObject el in tmpHouses)
        {
            TownManager town = el.GetComponent<TownManager>();

            if(town != null)
            {
                townsOnMap.Add(town);
            }
        }


    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedPlayerTown != null && selectedTargetTown != null)
        {
            if(!selectedPlayerTown.HasAttacked)
            {
                Debug.Log("Launch Attack");
                selectedTargetTown.HasAttacked = true;

                selectedPlayerTown.spawnTroop(selectedTargetTown);

                selectedPlayerTown = null;
                selectedTargetTown = null;
            }

        }
    }

    private void UpdateComparisonSlider()
    {
        deathPlayerText.text = pondPlayer.ToString();
        deathEnemyText.text = pondEnemy.ToString();

        sliderComparison.value = (((float)pondPlayer) / ((float)pondEnemy + (float)pondPlayer));

    }



    public void CheckGameConditions()
    {
        if (!townsOnMap.Any(town => town.CurrentOwner == Owner.Enemy))
        {
            Debug.Log("VICTORY");
            WinCanvas.gameObject.SetActive(true);
        }
        else if (!townsOnMap.Any(town => town.CurrentOwner == Owner.Player) )
        {
            Debug.Log("FIN DE PARTIE");
            LoseCanvas.gameObject.SetActive(true);
        }


    }

    public void DepleteTroop(Owner currentOwner)
    {
        if (currentOwner == Owner.Player)
        {
            nbDeathPlayer++;
            pondPlayer += Random.Range(80, 120);
        }
        else if (currentOwner == Owner.Enemy)
        {
            nbDeathEnemy++;
            pondEnemy += Random.Range(40, 60);
        }
        UpdateComparisonSlider();

    }

    public void FinishedAttack(TownManager selectedTown)
    {
        selectedTown.HasAttacked = false;
    }

    public void SelectTown(TownManager selectedTown)
    {
        if(selectedPlayerTown == null)
        {
            SelectPlayerTown(selectedTown);
        }
        else if(selectedPlayerTown == selectedTown)
        {
            selectedPlayerTown = null;
        }
        else
        {
            SelectTargetTown(selectedTown);
        }
    }

    private void SelectPlayerTown(TownManager selectedTown)
    {
        if(selectedTown.CurrentOwner == Owner.Player)
        {
            Debug.Log("Player Town Selected");
            selectedPlayerTown = selectedTown;
        }

    }

    private void SelectTargetTown(TownManager selectedTown)
    {
        if (selectedPlayerTown != null)
        {
            Debug.Log("Target Town Selected");
            selectedTargetTown = selectedTown;
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownManager : MonoBehaviour
{
    public SpawnScriptable dataSpawn;
    
    private float timerGenerateTroop = 0f;
    private int currentTroop;
    public int CurrentTroop { get { return currentTroop; } }

    public bool HasAttacked { get; set; } = false;
    public Owner CurrentOwner { get; private set; } = Owner.Unknown;

    #region variablesReferences

    private SpriteRenderer sprite;
    private Canvas canvas;
    private Text textTroop;
    
    #endregion

    #region variablesForUpdateSendTroopTarget

    GameObject instantiateEntity;
    GameObject resourcesTroopEntity;
    private TownManager sendTroopTarget = null;
    private int troopToSend = 0;
    private float timerSendTroop = 0f;
    #endregion




    void Start()
    {
        CurrentOwner = dataSpawn.who;
        currentTroop = dataSpawn.troopInit;

        sprite = GetComponent<SpriteRenderer>();
        canvas = GetComponentInChildren<Canvas>();
        textTroop = GetComponentInChildren<Text>();

        instantiateEntity = GameObject.Find("_Instantiate");
        resourcesTroopEntity = Resources.Load<GameObject>("TroopEntity");
    }

    void Update()
    {
        if(!HasAttacked)
            UpdateGenerateTroop();
        UpdateSendTroopTarget();

        UpdateColorSprite();
        UpdateTextNumberTroop();
    }

    #region UILoops
    public void UpdateTextNumberTroop()
    {
        textTroop.text = currentTroop.ToString();
    }

    public void UpdateColorSprite()
    {
        Color tmpColor;

        switch (CurrentOwner)
        {
            case Owner.Player:
                tmpColor = Color.blue;
                break;
            case Owner.Enemy:
                tmpColor = Color.red;
                break;
            case Owner.Neutral:
                tmpColor = Color.gray;
                break;
            default:
                tmpColor = Color.magenta;
                break;
        }

        sprite.color = tmpColor;

    }

    #endregion

    #region GameLoops
    private void UpdateGenerateTroop()
    {
        if (CurrentOwner == Owner.Neutral) return; //Neutral villagers don't generate units
        
        if (currentTroop < dataSpawn.troopMax)
        {
            if (timerGenerateTroop >= 2.5f)
            {
                currentTroop++;
                timerGenerateTroop = 0f;
            }
            else
            {
                timerGenerateTroop += Time.deltaTime;
            }
        }
    }

    public void UpdateSendTroopTarget()
    {
        if(sendTroopTarget != null)
        {
            if(troopToSend != 0)
            {

                if (timerSendTroop >= 0.75f)
                {
                    GameObject troopEntity = Instantiate(resourcesTroopEntity);

                    troopEntity.transform.parent = instantiateEntity.transform;
                    troopEntity.transform.position = this.transform.position;

                    Troop tmpTroop = troopEntity.GetComponent<Troop>();

                    if(tmpTroop != null)
                    {
                        tmpTroop.currentOwner = CurrentOwner;
                        tmpTroop.TargetTown = sendTroopTarget;
                    }

                    troopToSend--;

                    timerSendTroop = 0f;
                }
                else
                {
                    timerSendTroop += Time.deltaTime;
                }


            }
            else
            {
                GameManager.Instance.FinishedAttack(sendTroopTarget);
                sendTroopTarget = null;
            }

        }
    }

    #endregion

    public void TroopAttack(Troop troop)
    {
        if(troop.currentOwner != CurrentOwner)
        {
            //Under attack
            currentTroop--;


            GameManager.Instance.DepleteTroop(CurrentOwner);
            GameManager.Instance.DepleteTroop(troop.currentOwner);

            if (currentTroop <= 0)
            {
                changeOwner(troop.currentOwner);
                HasAttacked = false;
                GameManager.Instance.CheckGameConditions();
            }
        }
        else
        {
            currentTroop++;
        }

    }

    public void spawnTroop(TownManager target)
    {
        sendTroopTarget = target;
        if(currentTroop > 1)
        {
            int tmpTroopSend = currentTroop - 1;

            currentTroop -= tmpTroopSend;


            troopToSend = tmpTroopSend;
        }
    }

    public void changeOwner(Owner newOwner)
    {
        CurrentOwner = newOwner;
    }

    public void OnMouseDown()
    {
        //Debug.Log("OnMouseDown()");

        GameManager.Instance.SelectTown(this);

    }

}

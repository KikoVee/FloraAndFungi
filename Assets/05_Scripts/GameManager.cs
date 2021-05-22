using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager currentManager;

   // public GameObject currentPlayer;
    public HexMapEditor _hexMapEditor;
    private NutrientManager _nutrientManager;
    private int sugarScore;
    [SerializeField] private TextMeshProUGUI sugarScoreText;
    private int nutrientScore;
    [SerializeField] private TextMeshProUGUI nutrientScoreText;

    public delegate void EndTurnEvent();         //when player ends the turn it calls all other onTurnEnd events from other scripts
    public static EndTurnEvent onTurnEnd;

    public delegate void GiveNutrientsEvent();

    public static GiveNutrientsEvent nutrientEvent;

    public Transform fungiPrefab;

    public bool turnEndSequence;
    private float timer;
    private float time = 0.2f;
    public List<Transform> touchedTrees = new List<Transform>();
 
    
    private void Awake()
    {
        if (currentManager == null)
        {
            currentManager = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _nutrientManager = NutrientManager.currentNutrientManager;
    }

    private void Update()
    {
        if (turnEndSequence && timer > 0)
        {
            timer -= Time.deltaTime;
            //Debug.Log("timer is " + timer);
        }
        else
        {
            turnEndSequence = false;
        }
    }

    public void UpdateSugarScore(int sugar)
    {
        sugarScore = sugar;
        sugarScoreText.text = "Sugar:" + sugarScore;
    }
    public void UpdateNutrientScore(int nutrient)
    {
        nutrientScore = nutrient;
        nutrientScoreText.text = "N:" + nutrientScore;
    }
    public void ShowCostOfUpgrade()
    {
        int _nutrientCost = _nutrientManager.nutrientCost;
        nutrientScoreText.text = "S:" + _nutrientCost;
    }
    public void ShowUpgradeState()
    {
        nutrientScoreText.text = "N:" + nutrientScore;
    }
   
    public IShopCustomer getCustomer(IShopCustomer shopCustomer)
    {
        shopCustomer = NutrientManager.currentNutrientManager.GetComponent<IShopCustomer>();
        return shopCustomer;
    }

    public void EndTurn()
    {
        GiveTreesNutrients();
        //begins revalue cycle for all trees and skips time ahead quickly
        if (onTurnEnd != null)
        {
            onTurnEnd();
        }
        
        turnEndSequence = true;
        timer = time;

    }

    public void GiveTreesNutrients()
    {
        if (nutrientEvent != null)
        {
            nutrientEvent();
        }
    }

    public int GetCurrentNutrientValue()
    {
        int nutrientValue = gameObject.GetComponent<NutrientManager>().nutrientAmount;
        return nutrientValue;
    }

    public List<Transform> getTouchedTrees()
    {

        return touchedTrees;
    }

    public int GetCurrentTouchedTreeCount()
    {
        int treeCount = touchedTrees.Count;
        return treeCount;
    }

    
    

   
}

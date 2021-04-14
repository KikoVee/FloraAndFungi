using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager currentManager;

    public GameObject currentPlayer;
    private int sugarScore;
    [SerializeField] private TextMeshProUGUI sugarScoreText;
    private int nitrateScore;
    [SerializeField] private TextMeshProUGUI nitrateScoreText;

    public delegate void EndTurnEvent();         //when player ends the turn it calls all other onTurnEnd events from other scripts
    public static EndTurnEvent onTurnEnd;

    public delegate void GiveNutrientsEvent();

    public static GiveNutrientsEvent nutrientEvent;
 
    
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
   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSugar(int increase)
    {
        sugarScore += increase;
        sugarScoreText.text = "Sugar:" + sugarScore;
    }
    public void AddNitrate(int increase)
    {
        nitrateScore += increase;
        nitrateScoreText.text = "Nitrate: " + nitrateScore;
    }

    public IShopCustomer getCustomer(IShopCustomer shopCustomer)
    {
        shopCustomer = currentPlayer.GetComponent<IShopCustomer>();
        return shopCustomer;
    }

    public void EndTurn()
    {
        //begins revalue cycle for all trees and skips time ahead quickly
        if (onTurnEnd != null)
        {
            onTurnEnd();
        }

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
        int nutrientValue = gameObject.GetComponent<NutrientManager>().nutrientToSpend;
        return nutrientValue;
    }
}

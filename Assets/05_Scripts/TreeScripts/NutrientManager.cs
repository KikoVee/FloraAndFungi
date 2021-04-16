using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientManager : MonoBehaviour
{
    //this object keeps track of the amount of nutrient and sugar the player has to work with
    public int nutrientToSpend;
    public int sugarToSpend;

    public int expansionCost = 5;
    public int giveNutrientCost = 20;
    
    public static NutrientManager currentNutrientManager;
    
    private void Awake()
    {
        if (currentNutrientManager == null)
        {
            currentNutrientManager = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public bool TrySpendSugarAmount(int cost) //checks if enough sugar
    {
        if (sugarToSpend >= cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpendSugar(int cost)
    {
        sugarToSpend -= cost;
    }

    public void AddSugar(int sugar)
    {
        sugarToSpend += sugar;
    }
}

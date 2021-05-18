using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientManager : MonoBehaviour , IShopCustomer 
{
    //this object keeps track of the amount of nutrient and sugar the player has to work with
    public int currentNutrient;
    public int currentSugar;

    public int expansionCost = 5;
    public int nutrientCost = 10;
    
    public static NutrientManager currentNutrientManager;
    private GameManager _gameManager;
    
    private bool storeOpen;
    [SerializeField] private UIShop uiShop;
    
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

    private void Start()
    {
        _gameManager = GameManager.currentManager;
        _gameManager.UpdateSugarScore(currentSugar);
        _gameManager.UpdateNutrientScore(currentNutrient);
        GameManager.onTurnEnd += NewCycleSugar;
    }

    private void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.E) && _gameManager.turnEndSequence != true)
        {
            IShopCustomer shopCustomer = gameObject.GetComponent<IShopCustomer>();

            if (!storeOpen)
            {
                uiShop.Show(shopCustomer);
                storeOpen = true;
            }
            else
            {
                uiShop.Hide();
                storeOpen = false;
            }    
        }*/
    }

    

    public bool TrySpendSugarAmount(int cost) //checks if enough sugar
    {
        if (currentSugar >= cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void BoughtItem(UpgradeTypes.ItemType itemType)
    {
        Debug.Log("upgraded with " + itemType);
        switch (itemType)
        {
            case UpgradeTypes.ItemType.Nutrient:  AddNutrient(1); break;
            //case UpgradeTypes.ItemType.Expansion:  AddExpansion(); break;
 
        }
    }

    public void SpendSugar(int cost)
    {
        currentSugar -= cost;
        _gameManager.UpdateSugarScore(currentSugar);
    }

    public void AddSugar(int sugar)
    {
        this.currentSugar += sugar;
        _gameManager.UpdateSugarScore(currentSugar);
    }

    public void AddNutrient(int _nutrient)
    {
        currentNutrient += _nutrient;
        SpendSugar(nutrientCost);
        _gameManager.UpdateNutrientScore(currentNutrient);
    }

    private void NewCycleSugar()
    {
        _gameManager.UpdateSugarScore(currentSugar);
    }

    public void BuyNutrientsButton()
    {
        if (TrySpendSugarAmount(nutrientCost))
        {
            AddNutrient(1);
        }
    }
    
}

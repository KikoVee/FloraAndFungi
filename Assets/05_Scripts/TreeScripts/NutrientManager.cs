using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientManager : MonoBehaviour , IShopCustomer 
{
    //this object keeps track of the amount of nutrient and sugar the player has to work with
    public int nutrient;
    public int sugar;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _gameManager.turnEndSequence != true)
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
        }
    }

    

    public bool TrySpendSugarAmount(int cost) //checks if enough sugar
    {
        if (sugar >= cost)
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
        sugar -= cost;
    }

    public void AddSugar(int sugar)
    {
        this.sugar += sugar;
    }

    public void AddNutrient(int nutrient)
    {
        this.nutrient += nutrient;
        SpendSugar(nutrientCost);
    }
    
    
}

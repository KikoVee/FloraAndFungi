using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class NutrientManager : MonoBehaviour , IShopCustomer 
{
    //this object keeps track of the amount of nutrient and sugar the player has to work with
    private int nutrientScore;
    public int nutrientAmount;
    public int currentSugar;

    public int expansionCost = 5;
    public int nutrientCost = 10;
    public int nutrientUpgradeAmount = 10;
    
    public delegate void NutrientEvent();
    public static NutrientEvent addNutrientEvent;

    
    public static NutrientManager currentNutrientManager;
    private GameManager _gameManager;

    [SerializeField] public Image nutrientButtonMask;
    [SerializeField] public Button upgradeButton;
    private float differenceTillUpgrade;
    private float fillAmount;
    private float oldFillAmount;
    private bool updateButtonVisual;
    
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
        _gameManager.UpdateNutrientScore(nutrientScore);
        GameManager.onTurnEnd += NewCycleSugar;
        float maximumOffset = nutrientCost;
        float currentOffset = currentSugar;
        fillAmount = currentOffset/ maximumOffset;
        UpdateNutrientVisual();


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


        if (updateButtonVisual)
        {
            
            nutrientButtonMask.fillAmount = Mathf.Lerp(oldFillAmount, fillAmount, 3f * Time.deltaTime);
            updateButtonVisual = false;
        }
                
        
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
        UpdateNutrientVisual();

    }

    public void AddSugar(int sugar)
    {
        this.currentSugar += sugar;
        _gameManager.UpdateSugarScore(currentSugar);
        UpdateNutrientVisual();
    }

    public void AddNutrient(int upgrade)
    {
        nutrientScore += upgrade;
        SpendSugar(nutrientCost);
        nutrientCost += 10;
        nutrientAmount += nutrientUpgradeAmount;
        _gameManager.UpdateNutrientScore(nutrientScore);
        
        if (addNutrientEvent != null)
        {
            addNutrientEvent();
        }
        
       // NutrientLevels();
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

    public void BuyExpansion()
    {
        SpendSugar(expansionCost);
        //expansionCost += 1;
    }

    /*private void NutrientLevels() //divides up the nutrients based on number of trees around
    {
        int treeNumber = _gameManager.touchedTrees.Count;

        if (treeNumber > 0)
        {
            nutrientAmount = nutrientAmount / treeNumber;
        }
    }*/

    public void NutrientLevelSplit()
    {
        int treeNumber = _gameManager.touchedTrees.Count;

        if (treeNumber > 0)
        {
            nutrientAmount = nutrientAmount / treeNumber;
        }
    }

    private void UpdateNutrientVisual()
    {
        oldFillAmount = fillAmount;
        float maximumOffset = nutrientCost-1;
        float currentOffset = currentSugar-1;
        fillAmount = currentOffset/ maximumOffset;
        updateButtonVisual = true;

    }
    
    // expand map with more fungi

    
    
}

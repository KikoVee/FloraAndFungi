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
    [SerializeField] private int undividedNutrientAmount;
    public int currentSugar;

    public int expansionCost = 5;
    public int nutrientCost = 10;
    public int nutrientUpgradeAmount = 10;
    [SerializeField] private int fungiCount;
    
    public delegate void NutrientEvent();
    public static NutrientEvent addNutrientEvent;
    
    
    public static NutrientManager currentNutrientManager;
    private GameManager _gameManager;

    [SerializeField] public Image nutrientButtonMask;
    [SerializeField] public GameObject upgradeButton;
    private float differenceTillUpgrade;
    private float fillAmount;
    private float oldFillAmount;
    private bool updateButtonVisual;

    private bool tutorial;
   
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
       // UpdateNutrientVisual();


    }

    private void Update()
    {
       

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
       // UpdateNutrientVisual();

    }

    public void AddSugar(int sugar)
    {
        this.currentSugar += sugar;
        _gameManager.UpdateSugarScore(currentSugar);
      //  UpdateNutrientVisual();
    }

    public void AddNutrient(int upgrade)
    {
        nutrientScore += upgrade;
        SpendSugar(nutrientCost);
        nutrientCost += 10;
        nutrientAmount += nutrientUpgradeAmount;
        _gameManager.UpdateNutrientScore(nutrientAmount);
        _gameManager.GiveTreesNutrients();
        FindObjectOfType<AudioManager>().Play("Spores");

        
        
        if (addNutrientEvent != null)
        {
            addNutrientEvent();
        }
        
       // NutrientLevels();
    }

    private void NewCycleSugar()
    {
        int sugar = Mathf.RoundToInt(currentSugar - (fungiCount * .5f));
        currentSugar = Mathf.Clamp(sugar,-20, 1000);
        _gameManager.UpdateSugarScore(currentSugar);

        if (currentSugar < 0)
        {
            //kill some of fungi
            
        }
    }

    public void BuyNutrientsButton()
    {
        if (TrySpendSugarAmount(nutrientCost))
        {
            AddNutrient(1);
            upgradeButton.SetActive(false);
        }
    }

    public void BuyExpansion()
    {
        SpendSugar(expansionCost); 
        _gameManager.ExpandedNetwork();
        
        //adding nutrients with each expansion
        nutrientAmount += 1;
        undividedNutrientAmount += 1;
        fungiCount += 1;
        _gameManager.UpdateNutrientScore(nutrientAmount);

    }

    public void NutrientLevelSplit(int tree)
    {
        
        int treeNumber = _gameManager.touchedTrees.Count;
        int nutrientValue = undividedNutrientAmount;

         if (treeNumber > 0)
         {
             
             nutrientValue = nutrientValue / treeNumber;
             nutrientAmount = nutrientValue;
             _gameManager.UpdateNutrientScore(nutrientAmount);
         }

    }

    /*private void UpdateNutrientVisual()
    {
        oldFillAmount = fillAmount;
        float maximumOffset = nutrientCost-1;
        float currentOffset = currentSugar-1;
        fillAmount = currentOffset/ maximumOffset;
        updateButtonVisual = true;
        
        if (currentSugar >= nutrientCost)
        {
            upgradeButton.SetActive(true); 
        }
        else
        {
            upgradeButton.SetActive(false); 
        }

    }*/
    
     

    private void TutorialPopUp()
    {
        //add popup explanation about tree upgrades when ready to upgrade;
    }
    
    // expand map with more fungi

    
    
}

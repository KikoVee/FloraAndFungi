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
    [SerializeField] private float sugarConsumption = .1f;
    [SerializeField] private int undividedNutrientAmount;
    public int currentSugar;
    private int sugarNeededConsumption;


    public int expansionCost = 5;
    public int nutrientCost = 10;
    public int nutrientUpgradeAmount = 10;
    [SerializeField] private int fungiCount;
    
    //public delegate void NutrientEvent();
    //public NutrientEvent addNutrientEvent;
    
    
    public static NutrientManager currentNutrientManager;
    private GameManager _gameManager;

    [SerializeField] public Image nutrientButtonMask;
    [SerializeField] public GameObject upgradeButton;
    private float differenceTillUpgrade;
    private float fillAmount;
    private float oldFillAmount;
    private bool updateButtonVisual;
    [SerializeField] private Text SugarText;

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
        //GameManager.onTurnEnd += NewCycleSugar;
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

    public void UpdateSugarNeededText()
    {
        sugarNeededConsumption = Mathf.RoundToInt(fungiCount * sugarConsumption);
        SugarText.text = ("This is the amount of sugar your fungal network has. Sugar is energy for the fungi and is given by the trees. " + 
                          "\n" + "\n" +
                          "Your fungi currently need to consume " + sugarNeededConsumption + " sugar.");
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
        
        if (_gameManager._tutorialManager.tutorial == true)
        {
            _gameManager._tutorialManager.collectedSugar = true;
        }
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
    }

    

    public void NewCycleSugar()
    {
        if (_gameManager.timelapse != true && _gameManager.gameOver != true && _gameManager.gameWin != true)
        {
            int sugar = Mathf.RoundToInt(currentSugar - (fungiCount * sugarConsumption));
            currentSugar = Mathf.Clamp(sugar, -fungiCount, 1000);
            _gameManager.UpdateSugarScore(currentSugar);

            if (currentSugar < 0)
            {
                //kill some of fungi
                _gameManager.UnhealthyFungi(currentSugar);
            }
            else
            {
                _gameManager.HealthyFungi();
            } 
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

    public void HealthyFungiCount(int count)
    {
        undividedNutrientAmount = count; 
        NutrientLevelSplit();
    }

    public void NutrientLevelSplit() //divides nutrients among trees
    {
        
        int treeNumber = _gameManager.touchedTrees.Count;
        int nutrientValue = undividedNutrientAmount;

         if (treeNumber > 0)
         {
             nutrientValue = nutrientValue / treeNumber;
         }
        nutrientAmount = nutrientValue;
        _gameManager.UpdateNutrientScore(nutrientAmount);

    }

    private void TutorialPopUp()
    {
        //add popup explanation about tree upgrades when ready to upgrade;
    }
    
    // expand map with more fungi

    
    
}

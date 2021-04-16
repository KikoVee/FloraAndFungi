using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class PlayerBehaviour : MonoBehaviour , IShopCustomer
{
    private GameManager _gameManager;

    private bool storeOpen;
    [SerializeField] private UIShop uiShop;

    private int sugarAmount;
    private int nutrientAmount;
    private int expansionCost = 1;

    public GameObject nutrientObject;

    private bool recordMovements = false;
    public GameObject wayPointObject;
    public List<Transform> wayPointList;
    private float timer;
    private float timeBetweenWaypoints = 2f;

    private void Start()
    {
        _gameManager = GameManager.currentManager;
        wayPointList = new List<Transform>();
        timer = timeBetweenWaypoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        if (Input.GetMouseButtonDown(1))
        {
            UseNitrogen();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.currentManager.GiveTreesNutrients();
        }

    }
    

    private void ChangeCellColor(Vector3 pos)
    {
        if (TrySpendSugarAmount(expansionCost))
        {
            //hexGrid.ColorCell(pos, rangeColor);
            ChangeSugarAmount(-expansionCost);  
            Debug.Log("change color");
        }
        else
        {
            Debug.Log("not enough sugar");
        }
    }
    
    public void ChangeSugarAmount(int sugar)
    {
        sugarAmount += sugar;
    }
    public void AddNitrate(int nitrate)
    {
        nutrientAmount += nitrate;
    }

    

    public void BoughtItem(UpgradeTypes.ItemType itemType)
    {
        Debug.Log("bought item " + itemType);
        //insert the values for when you buy the item
        switch (itemType)
        {
            case UpgradeTypes.ItemType.Nutrient:  AddNitrate(1); break;
            //case UpgradeTypes.ItemType.Expansion:  AddExpansion(); break;
 
        }
        //add warning if can't afford
        
    }

    public bool TrySpendSugarAmount(int spendSugarAmount)
    {
        if (sugarAmount >= spendSugarAmount)
        {
            sugarAmount -= spendSugarAmount;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UseNitrogen()
    {
        if (nutrientAmount > 0)
        {
            nutrientAmount -= 1;
            Instantiate(nutrientObject, gameObject.transform.position, Quaternion.identity);
            Debug.Log("use nitrate");
        }
        else
        {
            Debug.Log("not enough Nitrate");
        }
    }

    
    
}

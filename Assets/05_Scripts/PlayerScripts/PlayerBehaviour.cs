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
    private int nitrateAmount;

    public GameObject nitrateObject;

    private Transform spawnPoint;
    public HexGrid hexGrid;
    private Color rangeColor = Color.green;

    private bool recordMovements = false;
    public GameObject wayPointObject;
    public List<Transform> wayPointList;
    private float timer;
    private float timeBetweenWaypoints = 2f;

    private void Start()
    {
        _gameManager = GameManager.currentManager;
        spawnPoint = gameObject.transform;
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
        
    }
    
    private void FixedUpdate()
    {
        GetLocationOnGrid();
        
        if (recordMovements == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                AddWayPoint();
                timer = timeBetweenWaypoints;
            }
        }
    }

    public void GetLocationOnGrid()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Vector3 pos = hit.point;
            HexCell currentCell;
            currentCell = hexGrid.GetCell(pos);
            if (currentCell.walkable != true)
            {
                ChangeCellColor(pos);
                // Debug.Log("current cell is " + currentCell + "is walkable");
            }
        }
    }

    private void ChangeCellColor(Vector3 pos)
    {
        hexGrid.ColorCell(pos, rangeColor);
    }
    
    public void AddSugar(int sugar)
    {
        sugarAmount += sugar;
    }
    public void AddNitrate(int nitrate)
    {
        nitrateAmount += nitrate;
    }
    public void AddExpansion()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Collectables collectable = other.gameObject.GetComponent<Collectables>();
        
        if (collectable != null)
        {
            int points = collectable.pointValue;
            _gameManager.AddSugar(points);
            AddSugar(points);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("RecordStart"))
        {
            recordMovements = true;
            Debug.Log(recordMovements);
        }

        if (other.gameObject.CompareTag("RecordEnd"))
        {
            recordMovements = false;
            Debug.Log(recordMovements);
            gameObject.GetComponent<PlayerMovementReplay>().BeginReplay();

        }
    }

    public void BoughtItem(UpgradeTypes.ItemType itemType)
    {
        Debug.Log("bought item " + itemType);
        //insert the values for when you buy the item
        switch (itemType)
        {
            case UpgradeTypes.ItemType.Nitrate:  AddNitrate(1); break;
            case UpgradeTypes.ItemType.Expansion:  AddExpansion(); break;
 
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
        if (nitrateAmount > 0)
        {
            nitrateAmount -= 1;
            Instantiate(nitrateObject, gameObject.transform.position, Quaternion.identity);
            Debug.Log("use nitrate");
        }
        else
        {
            Debug.Log("not enough Nitrate");
        }
    }

    private void AddWayPoint()
    {
        GameObject newWayPoint = Instantiate(wayPointObject, transform.position, transform.rotation);
        Transform wayPoint = newWayPoint.transform;
        wayPointList.Add(wayPoint);
    }
    
}

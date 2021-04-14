using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    private int maxTreeHealth = 100;
    [SerializeField] private int currentTreeHealth;
    
    [SerializeField] private int treeNutrientWeight;
    [SerializeField] private int treeWeatherWeight;

    [SerializeField] private int sugarValue;
    [SerializeField] private int treeSugarWeight;
    [SerializeField] private int treeSugarWeatherWeight;

    private int weatherValue;
   [SerializeField] private int currentNutrientValue;
    private bool isDead = false;

    private int rangeMin = 3;
    private int rangeMax = 3;
    [SerializeField] private GameObject sugarPrefab;

    [SerializeField]private HexCell currentCell;
    public HexGrid hexGrid;
    private int currentNutrientAmount;


    // Start is called before the first frame update
    void Start()
    {
        GetCellLocation();
        currentTreeHealth = maxTreeHealth;
        GameManager.onTurnEnd += NewCycle;
        GameManager.nutrientEvent += GiveNutrients;
    }

    void GetCellLocation()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            currentCell = hexGrid.GetCell(hit.point);
            currentCell.SetType(1);
            Debug.Log("current cell for tree is " + currentCell);
        }
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) *hit.distance, Color.red);

    }

    // Update is called once per frame
    void Update()
    {
        if (currentTreeHealth <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }

        if (currentCell == null)
        {
            GetCellLocation();
        }

    }



    /*void TakeHealth(int health)
    {
        if (currentTreeHealth <= maxTreeHealth)
        {
            currentTreeHealth += health;
        }
        else
        {
            Debug.Log("tree is max health");
        }
    }

    void TakeDamage(int damage)
    {
        currentTreeHealth -= damage;
    }*/

    public void NewCycle()
    {
        currentTreeHealth = currentNutrientValue * treeNutrientWeight + weatherValue * treeWeatherWeight; // sets the tree health based on the amount of nutrients available and the weather
        sugarValue = currentTreeHealth * treeSugarWeight - weatherValue * treeSugarWeatherWeight; //sets the amount of sugar tree produces based on health of tree
        int newNutrientValue = currentNutrientValue - 5; //gradual decrease in nutrients 
        currentNutrientValue = newNutrientValue; //sets current nutrient value to the new value
        
        //SpawnSugar();
    }

    public void GiveNutrients()
    {
        //check if neighboring cells are fungi, if true then add sugar
        HexCell[] neighbors = currentCell.GetNeighbors();

        foreach (HexCell cell in neighbors)
        {
            if (cell.myType == HexCell.cellType.fungi)
            {
                currentNutrientAmount = GameManager.currentManager.GetCurrentNutrientValue();
                currentNutrientValue += currentNutrientAmount;
            }
        }
        
       
    }

    void SpawnSugar()
    {
        for (int i = 0; i < sugarValue; i++)
        {
            //Debug.Log("i value is " + i);
            Vector3 center = transform.position;
            var pos = new Vector3(Random.Range((center.x -rangeMin), (center.z + rangeMax)), 0, Random.Range((center.x-rangeMin), (center.z +rangeMax)));
            GameObject newSugar = Instantiate(sugarPrefab, pos, Quaternion.Euler(0,Random.Range(0,360),0)); 
        }
        Debug.Log("spawning sugar has finished");  
    }
  
}

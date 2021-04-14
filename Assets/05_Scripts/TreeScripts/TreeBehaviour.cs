using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    private int maxTreeHealth = 100;
    [SerializeField] private int currentTreeHealth;
    
    [SerializeField] private int treeNitrateWeight;
    [SerializeField] private int treeWeatherWeight;

    [SerializeField] private int sugarValue;
    [SerializeField] private int treeSugarWeight;
    [SerializeField] private int treeSugarWeatherWeight;

    private int weatherValue;
   [SerializeField] private int currentNitrateValue;
    private int newNitrateValue;
    private bool isDead = false;

    private int rangeMin = 3;
    private int rangeMax = 3;
    [SerializeField] private GameObject sugarPrefab;

    private HexCell currentCell;
    public HexGrid hexGrid;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        GetCellLocation();
        currentTreeHealth = maxTreeHealth;
        GameManager.onTurnEnd += NewCycle;
        
    }

    void GetCellLocation()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            currentCell = hexGrid.GetCell(hit.point);
            Debug.Log("current cell for tree is " + currentCell);
        }
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
        
    }



    void TakeHealth(int health)
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
    }

    public void NewCycle()
    {
        currentTreeHealth = currentNitrateValue * treeNitrateWeight + weatherValue * treeWeatherWeight;
        sugarValue = currentTreeHealth * treeSugarWeight - weatherValue * treeSugarWeatherWeight;
        newNitrateValue = currentNitrateValue - 5;
        currentNitrateValue = newNitrateValue;
        
        SpawnSugar();
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

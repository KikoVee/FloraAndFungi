﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    private int maxTreeHealth = 100;
    [SerializeField] private float currentTreeHealth;
    
    [SerializeField] private float treeNutrientWeight;
    [SerializeField] private float treeWeatherWeight;

    [SerializeField] private float sugarValue;
    [SerializeField] private float treeSugarWeight;
    [SerializeField] private float treeSugarWeatherWeight;

    private int weatherValue;
   [SerializeField] private int currentNutrientValue;
    private bool isDead = false;

    private int rangeMin = 3;
    private int rangeMax = 3;
    [SerializeField] private GameObject sugarPrefab;

    [SerializeField]private HexCell currentCell;
    public HexGrid hexGrid;
    private int currentNutrientAmount;
    private NutrientManager _nutrientManager;

    public Material[] treeMaterial;
    private Renderer treeRenderer;


    // Start is called before the first frame update
    void Start()
    {
        GetCellLocation();
        currentTreeHealth = maxTreeHealth;
        GameManager.onTurnEnd += NewCycle;
        GameManager.nutrientEvent += GetNutrients;
        _nutrientManager = NutrientManager.currentNutrientManager;
        treeRenderer = gameObject.GetComponentInChildren<Renderer>();
        TreeVisualChange();
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

    public void NewCycle()
    {
        
        currentTreeHealth = Mathf.Clamp(currentNutrientValue * treeNutrientWeight + weatherValue * treeWeatherWeight, 0, 100); // sets the tree health based on the amount of nutrients available and the weather
        sugarValue = Mathf.Clamp(currentTreeHealth * treeSugarWeight - weatherValue * treeSugarWeatherWeight, 0, 100); //sets the amount of sugar tree produces based on health of tree
        GiveSugar();
        int newNutrientValue = Mathf.Clamp(currentNutrientValue - 5, 0, 100); //gradual decrease in nutrients 
        currentNutrientValue = newNutrientValue; //sets current nutrient value to the new value
        TreeVisualChange();
        
        //SpawnSugar();
    }

    public void GetNutrients()
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

    private void GiveSugar()
    {
        for (int i = 0; i < sugarValue; i++)
        {
            _nutrientManager.AddSugar(1);
            sugarValue -= 1;
        }
    }

    void TreeVisualChange()
    {
      float  healthPercent = currentTreeHealth * 100;

        if (healthPercent >= 90)
        {
            treeRenderer.material = treeMaterial[0];
        }
        
        if (healthPercent >= 50)
        {
            treeRenderer.material = treeMaterial[1];
        }
        if (healthPercent <= 49)
        {
            treeRenderer.material = treeMaterial[2];
        }

        if (healthPercent <= 10)
        {
            treeRenderer.material = treeMaterial[3];

        }
      
    }

}

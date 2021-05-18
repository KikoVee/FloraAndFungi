﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreeBehaviour : MonoBehaviour
{
    private int maxTreeHealth = 100;
    [SerializeField] private float currentTreeHealth;
    
    [SerializeField] private float treeNutrientWeight;
    [SerializeField] private float treeWeatherWeight;

    [SerializeField] private float treeSugarValue;
    [SerializeField] private float treeSugarWeight;
    [SerializeField] private float treeSugarWeatherWeight;
    [SerializeField] private GameObject sugarObject;

    private float weatherValue;
   [SerializeField] private float currentNutrientValue;
    private bool isDead = false;

    private int rangeMin = 3;
    private int rangeMax = 3;
    [SerializeField] private GameObject sugarPrefab;

    [SerializeField]private HexCell currentCell;
    public HexGrid hexGrid;
    private int nutrientAmount;
    private NutrientManager _nutrientManager;
    private WeatherManager _weatherManager;

    public Material[] treeMaterial;
    [SerializeField] private Renderer treeRenderer;

    public TextMeshPro treeText;
    private bool fungiNeighbor = false;


    // Start is called before the first frame update
    void Start()
    {
        GetCellLocation();
        currentTreeHealth = maxTreeHealth;
        GameManager.onTurnEnd += NewCycle;
        GameManager.nutrientEvent += GetNutrients;
        _nutrientManager = NutrientManager.currentNutrientManager;
        _weatherManager = WeatherManager.currentWeatherManager;
        
        TreeVisualChange();
    }

    void GetCellLocation()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            currentCell = hexGrid.GetCell(hit.point);
            currentCell.SetType(1);
            currentCell.Color = Color.gray;
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

        treeText.text = "H: " + currentTreeHealth  
                        + " N: " + currentNutrientValue 
                        + " S: " + treeSugarValue;

    }

    public void NewCycle()
    {
        bool newCycle = true;

        if (newCycle)
        {
            CheckNeighbors();
            weatherValue = _weatherManager.weatherValue;
            currentTreeHealth =
                Mathf.Clamp((currentNutrientValue * treeNutrientWeight) + (weatherValue * treeWeatherWeight), 0,
                    100); // sets the tree health based on the amount of nutrients available and the weather
            treeSugarValue =
                Mathf.Clamp((currentTreeHealth * treeSugarWeight) - (weatherValue * treeSugarWeatherWeight), 0,
                    100); //sets the amount of sugar tree produces based on health of tree
            Debug.Log(this.gameObject + "sugar value is " + treeSugarValue);
            GiveSugar();
            float newNutrientValue =
                Mathf.Clamp(currentNutrientValue - (currentNutrientValue * 0.5f), 0,
                    100); //gradual decrease in nutrients 
            currentNutrientValue = newNutrientValue; //sets current nutrient value to the new valued
            TreeVisualChange();
            newCycle = false;
            //SpawnSugar();
        }
       
    }

    public void GetNutrients()
    {
        //check if neighboring cells are fungi, if true then add sugar
        HexCell[] neighbors = currentCell.GetNeighbors();

        foreach (HexCell cell in neighbors)
        {
            if (cell.myType == HexCell.cellType.fungi)
            {
                nutrientAmount = GameManager.currentManager.GetCurrentNutrientValue();
                currentNutrientValue += nutrientAmount;
            }
        }
         
    }

    private void GiveSugar()
    {
        if (fungiNeighbor)
        {
            for (int i = 0; i < treeSugarValue; i++)
            {
                _nutrientManager.AddSugar(1);
                 treeSugarValue -= 1;
            }
        }
    }

    void TreeVisualChange()
    {
      float  healthPercent = currentTreeHealth;

        if (healthPercent >= 90)
        {
            treeRenderer.material = treeMaterial[0];
        }
        
        if (healthPercent >= 50 && healthPercent <= 89)
        {
            treeRenderer.material = treeMaterial[1];
        }
        if (healthPercent >= 11 && healthPercent <= 49)
        {
            treeRenderer.material = treeMaterial[2];
        }

        if (healthPercent <= 10)
        {
            treeRenderer.material = treeMaterial[3];

        }
      
    }

    void CheckNeighbors()
    {
        HexCell[] neighbors = currentCell.GetNeighbors();

        foreach (HexCell cell in neighbors)
        {
            if (cell.myType == HexCell.cellType.fungi)
            {
                fungiNeighbor = true;
            }
        } 
    }

}

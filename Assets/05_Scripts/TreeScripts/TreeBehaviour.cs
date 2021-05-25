using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TreeBehaviour : MonoBehaviour
{
    private int maxTreeHealth = 100;
    [SerializeField] private float currentTreeHealth;
    
    [SerializeField] private float treeNutrientWeight;
    [SerializeField] private float treeWeatherWeight;

    [SerializeField] private int treeSugarValue;
    private float treeSugarWeight = 10;
    [SerializeField] private float treeSugarWeatherWeight;

    private float weatherValue;
   [SerializeField] private float currentNutrientValue;
    private bool isDead = false;

    [SerializeField]private int range = 3;
    [SerializeField]private int rangeMax = 3;
    [SerializeField] private GameObject sugarPrefab;

    [SerializeField]private HexCell currentCell;
    public HexGrid hexGrid;
    private int _nutrientAmount;
    [SerializeField] private NutrientManager _nutrientManager;
    private WeatherManager _weatherManager;

    public Material[] treeMaterial;
    public Material[] TreeLeavesMaterials;
    [SerializeField] private Renderer treeRenderer;
    public GameObject[] treeLeaves;
    private GameObject currentTreeLeaves;
    int treeLeavesNumber = 0;
    private int oldTreeLeavesNumber;
    public bool leaves;
    
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private float newBlendValue = 0;
    private float oldBlendValue = 0;
    private float blendSpeed = 3;
   
    
   

    public TextMeshPro treeText;
    private bool fungiNeighbor = false;

    private bool readToCollect = false;
    public Outline outline;


    // Start is called before the first frame update
    void Start()
    {
        GetCellLocation();
        GameManager.onTurnEnd += NewCycle;
        GameManager.nutrientEvent += GetNutrients;
        _nutrientManager = NutrientManager.currentNutrientManager;
        _weatherManager = WeatherManager.currentWeatherManager;
      
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMesh = _skinnedMeshRenderer.sharedMesh;
        newBlendValue = oldBlendValue;

        oldTreeLeavesNumber = treeLeavesNumber;
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
            //Debug.Log("current cell for tree is " + currentCell);
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

        if (readToCollect)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }

        if (oldBlendValue != newBlendValue)
        {
            _skinnedMeshRenderer.SetBlendShapeWeight(0, oldBlendValue);
            oldBlendValue = Mathf.Lerp(oldBlendValue, newBlendValue, blendSpeed * Time.deltaTime);
        }
        else
        {
            oldBlendValue = newBlendValue;
        }

        

        
        
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
                    100);
            /*treeSugarValue = Mathf.CeilToInt((currentTreeHealth - (weatherValue * treeSugarWeatherWeight)) / treeSugarWeight);
            treeSugarValue =
                //Mathf.CeilToInt((currentTreeHealth - (currentTreeHealth * treeSugarWeight)) - (weatherValue * treeSugarWeatherWeight)); //sets the amount of sugar tree produces based on health of tree
            if (treeSugarValue > 0 && fungiNeighbor)
            {
                readToCollect = true;
            }*/
            
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
        HexCell[] neighbors = currentCell.GetNeighbors();

        foreach (HexCell cell in neighbors)
        {
            if (cell.myType == HexCell.cellType.fungi)
            {
                _nutrientAmount = _nutrientManager.nutrientAmount;
                currentNutrientValue += _nutrientAmount;
            }
        }
         
    }


    void TreeVisualChange()
    {
      float  healthPercent = currentTreeHealth;
      oldTreeLeavesNumber = treeLeavesNumber;
      
      
        //Renderer[] rend = treeLeaves.GetComponentsInChildren<Renderer>();
        if (healthPercent >= 90)
        {
            treeRenderer.material = treeMaterial[0];
            newBlendValue = 0;
            treeLeavesNumber = 0;
            leaves = true;

            
            /*foreach (Renderer renderer in rend)
            {
                renderer.materials[0] = TreeLeavesMaterials[1];

            }*/

            treeSugarValue = 10;

        }
        
        if (healthPercent >= 50 && healthPercent <= 89)
        {
            treeRenderer.material = treeMaterial[1];
            newBlendValue = 30;
            leaves = true;
            treeLeavesNumber = 1;
            /*foreach (Renderer renderer in rend)
            {
                renderer.materials[0] = TreeLeavesMaterials[0];
            }*/

            treeSugarValue = 5;

        }
        if (healthPercent >= 11 && healthPercent <= 49)
        {
            treeRenderer.material = treeMaterial[2];
            newBlendValue = 60;
            leaves = false;

            treeSugarValue = 1;
        }

        if (healthPercent <= 10)
        {
            treeRenderer.material = treeMaterial[3];
            newBlendValue = 100;
            leaves = false;

            treeSugarValue = 0;
        }
        
        if (treeSugarValue > 0 && fungiNeighbor)
        {
            readToCollect = true;
        }

        if (leaves == true)
        {
            treeLeaves[treeLeavesNumber].SetActive(true);
            treeLeaves[oldTreeLeavesNumber].SetActive(false);
        }
        else
        {
            treeLeaves[treeLeavesNumber].SetActive(false);
            treeLeaves[oldTreeLeavesNumber].SetActive(false);
        }
        

    }

    void CheckNeighbors()
    {
        HexCell[] neighbors = currentCell.GetNeighbors();

        if (!fungiNeighbor)
        {
            foreach (HexCell cell in neighbors)
            {
                if (cell.myType == HexCell.cellType.fungi)
                {
                    fungiNeighbor = true;
                   // GameManager.currentManager.touchedTrees.Add(this);
                    Debug.Log(GameManager.currentManager.touchedTrees.Count);
                    break;
                }
            }  
        }
        
    }
   

    public void CollectSugar()
    {
        if (readToCollect)
        {
            _nutrientManager.AddSugar(treeSugarValue);
            treeSugarValue = 0;
            readToCollect = false;
        }
    }

    
    

}

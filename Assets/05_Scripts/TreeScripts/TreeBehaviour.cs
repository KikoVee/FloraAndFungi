using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeBehaviour : MonoBehaviour
{
    [Space] [Header("(Tree Traits)")]
    private int maxTreeHealth = 100;
    [SerializeField] private float currentTreeHealth = 20;
    [SerializeField] private float treeHealthWeight = 0.6f;
   // [SerializeField] private float treeNutrientWeight;
    //[SerializeField] private float treeWeatherWeight;
    [SerializeField] private int treeSugarValue;
    private float treeSugarWeight = 10;
    //[SerializeField] private float treeSugarWeatherWeight;
    [SerializeField] private float currentNutrientValue;

    private float weatherValue;
    private bool isDead = false;
    public enum TreeState {incomplete, complete};
    public TreeState treeState; 


    [Space] [Header("(Location and Managers)")]

    [SerializeField]private HexCell currentCell;
    public HexGrid hexGrid;
    private int _nutrientAmount;
    [SerializeField] private NutrientManager _nutrientManager;
    private WeatherManager _weatherManager;
    private CollectableAnimation _collectableManager;
    [SerializeField] private bool fungiNeighbor = false;


    [Space] [Header("(Visuals)")]

    public Material[] treeMaterial;
    public Material[] TreeLeavesMaterials;
    [SerializeField] private Renderer treeRenderer;
    public GameObject healthyLeaves;
    public GameObject unhealthyLeaves;
    private GameObject currentTreeLeaves;
    int treeLeavesNumber = 0;
    private int oldTreeLeavesNumber;
    public bool leaves;

    [SerializeField] private GameObject healthyTreeVisualsContainer;
    private SkinnedMeshRenderer[] healthyTreeDetails;
    
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private float newBlendValue = 0;
    private float oldBlendValue = 0;
    private float blendSpeed = 3;
   
    
   

    private Text treeText;

    private bool readToCollect = false;
    public Outline outline;
    [SerializeField] private GameObject particleContainer;
    private ParticleSystem[] upgradeParticles;


    // Start is called before the first frame update
    void Start()
    {
        GetCellLocation();
        //GameManager.onTurnEnd += NewCycle;
        //GameManager.nutrientEvent += GetNutrients;
       // GameManager.addExpansionEvent += CheckNeighbors;
        GameManager.currentManager.treesInScene.Add(this);

        _nutrientManager = NutrientManager.currentNutrientManager;
        _weatherManager = WeatherManager.currentWeatherManager;
        _collectableManager = GameManager.currentManager._sugarCollectableAnimation;
      
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMesh = _skinnedMeshRenderer.sharedMesh;
        newBlendValue = oldBlendValue;
        oldTreeLeavesNumber = treeLeavesNumber;
        treeText = gameObject.GetComponent<DisplayUI>().myText;

        healthyTreeDetails = healthyTreeVisualsContainer.GetComponentsInChildren<SkinnedMeshRenderer>();
        upgradeParticles = particleContainer.GetComponentsInChildren<ParticleSystem>();
        
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

        treeText.text = "Tree health is: " + Mathf.RoundToInt(currentTreeHealth);

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
           
            foreach (SkinnedMeshRenderer meshRenderer in healthyTreeDetails)
            {
                meshRenderer.SetBlendShapeWeight(0, oldBlendValue);
            }
            
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
                Mathf.Clamp((currentTreeHealth * treeHealthWeight) + (currentNutrientValue * 0.8f)+ (weatherValue), 0,
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
            SetTreeState(); //changes tree type based on amount of available nutrients
            TreeVisualChange();
            newCycle = false;
        }
       
    }

    public void GetNutrients()
    {
        HexCell[] neighbors = currentCell.GetNeighbors();

        if (GameManager.currentManager.fungiAlive)
        {
            foreach (HexCell cell in neighbors)
            {
                if (cell.myType == HexCell.cellType.fungi)
                {
                    _nutrientAmount = _nutrientManager.nutrientAmount;
                    currentNutrientValue += _nutrientAmount;
                    SetTreeState(); //changes tree type based on amount of available nutrients
                }
            }

            if (fungiNeighbor == true)
            {
                foreach (var particle in upgradeParticles)
                {
                    if (particle != null)
                    {
                        particle.Play();
                    }
                }

            }
        }    
    }


    void TreeVisualChange()
    {
      float  healthPercent = currentTreeHealth;
      oldTreeLeavesNumber = treeLeavesNumber;
      
        if (healthPercent >= 90)
        {
            if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.material = treeMaterial[0]; 
            }
            newBlendValue = 0;
            if (healthyLeaves != null && unhealthyLeaves != null)
            {
                healthyLeaves.SetActive(true);
                unhealthyLeaves.SetActive(false);  
            }
            
            treeSugarValue = 5;

        }
        
        if (healthPercent >= 50 && healthPercent <= 89)
        {
            if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.material = treeMaterial[1]; 
            }
            newBlendValue = 30;
            if (healthyLeaves != null && unhealthyLeaves != null)
            {
                unhealthyLeaves.SetActive(true);
                healthyLeaves.SetActive(false);
            }
            treeSugarValue = 3;

        }
        if (healthPercent >= 11 && healthPercent <= 49)
        {
            if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.material = treeMaterial[2]; 
            }
            newBlendValue = 60;
            if (healthyLeaves != null && unhealthyLeaves != null)
            {
                healthyLeaves.SetActive(false);
                unhealthyLeaves.SetActive(false);
            }
            
            treeSugarValue = 2;
        }

        if (healthPercent >= 1 && healthPercent <= 10)
        {
            if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.material = treeMaterial[3]; 
            }
            newBlendValue = 80;
            if (healthyLeaves != null && unhealthyLeaves != null)
            {
                healthyLeaves.SetActive(false);
                unhealthyLeaves.SetActive(false);
            }
            treeSugarValue = 1;
        }
        if (healthPercent <= 0)
        {
            if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.material = treeMaterial[3]; 
            }
            
            newBlendValue = 100;
            if (healthyLeaves != null && unhealthyLeaves != null)
            {
                healthyLeaves.SetActive(false);
                unhealthyLeaves.SetActive(false);
            }
            treeSugarValue = 0;
        }
        
        if (treeSugarValue > 0 && fungiNeighbor)
        {
            readToCollect = true;
        }

        
        

    }

    public void CheckNeighbors()
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
                    //Debug.Log(GameManager.currentManager.touchedTrees.Count);
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
            _collectableManager.AddCollectable(transform.position,treeSugarValue);
            FindObjectOfType<AudioManager>().Play("Chime");
            treeSugarValue = 0;
            readToCollect = false;
        }
    }

    public TreeState CurrentTreeState()
    {
        return treeState;
    }

    private void SetTreeState()
    {
        if (currentNutrientValue < 10)
        {
            treeState = TreeState.incomplete;
        }

        if (currentNutrientValue >= 10)
        {
            treeState = TreeState.complete;
        }
    }
    
    

}

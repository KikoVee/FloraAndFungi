using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class GameManager : MonoBehaviour
{
    public static GameManager currentManager;

   // public GameObject currentPlayer;
    public HexMapEditor _hexMapEditor;
    private NutrientManager _nutrientManager;
    private AudioManager _audioManager;
    public CollectableAnimation _sugarCollectableAnimation;
    private int sugarScore;
    [SerializeField] private Text sugarScoreText;
    private int nutrientScore;
    [SerializeField] private Text nutrientScoreText;
    [SerializeField] private Text nutrientCostText;
    [SerializeField] private Text treeScoreText;
    [SerializeField] private GameObject timeLapsePauseImage;
    [SerializeField] private Text timeLapsePauseText;

    private Color originalTextColor;

    public delegate void EndTurnEvent();         //when player ends the turn it calls all other onTurnEnd events from other scripts
    public static EndTurnEvent onTurnEnd;
    bool timeLapseClicked = false;
    [SerializeField] private GameObject timeLapseImage;


    public delegate void GiveNutrientsEvent();

    public static GiveNutrientsEvent nutrientEvent;

    public Transform[] fungiPrefab;

    public bool turnEndSequence;
    private bool timelapse = false;
    private float timer;
    private float time = 0.2f;
    public List<Transform> touchedTrees = new List<Transform>();
 
    
    private void Awake()
    {
        if (currentManager == null)
        {
            currentManager = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _nutrientManager = NutrientManager.currentNutrientManager;
        originalTextColor = nutrientCostText.color;
        _audioManager = FindObjectOfType<AudioManager>();
        FindObjectOfType<AudioManager>().Stop("Background Music");


    }

    private void Update()
    {
        if (turnEndSequence && timer > 0)
        {
            timer -= Time.deltaTime;
            //Debug.Log("timer is " + timer);
        }
        else
        {
            turnEndSequence = false;
        }

        if (timelapse && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timelapse && timer <= 0)
        {
           TimeLapse(); 
        }
    }

    public void UpdateSugarScore(int sugar)
    {
        sugarScore = sugar;
        sugarScoreText.text = "Sugar: " + sugarScore;
        ShowCostOfUpgrade();
    }
    
    public void UpdateNutrientScore(int nutrient)
    {
        nutrientScore = nutrient;
        nutrientScoreText.text = "Nutrient: " + nutrientScore;
        ShowCostOfUpgrade();
    }
    
    public void ShowCostOfUpgrade()
    {
        int _nutrientCost = _nutrientManager.nutrientCost;

        if (sugarScore >= _nutrientCost)
        {
            nutrientCostText.color = Color.green;
        }
        else
        {
            nutrientCostText.color = Color.red;
        }
        nutrientCostText.text = sugarScore + "/" + _nutrientCost;
    }
    
    
   

    public void EndTurn()
    {
        GiveTreesNutrients();
        //begins revalue cycle for all trees and skips time ahead quickly
        if (onTurnEnd != null)
        {
            onTurnEnd();
        }
        
        turnEndSequence = true;
        timer = time;
    }

    private void TimeLapse()
    {
        GiveTreesNutrients();

        if (onTurnEnd != null)
        {
            onTurnEnd();
        }

        timelapse = true;
        turnEndSequence = true;
        timer = time;
        timeLapseClicked = true;
        timeLapsePauseText.enabled = false;
        timeLapsePauseImage.SetActive(true);
        timeLapseImage.SetActive(true);


    }

    public void TimeLapseButton()
    {
        if (!timeLapseClicked)
        {
            TimeLapse();
        }
        else
        {
            timelapse = false;
            turnEndSequence = false;
            timer = 0;
            timeLapseClicked = false;
            timeLapsePauseText.enabled = true;
            timeLapsePauseImage.SetActive(false);
            timeLapseImage.SetActive(false);

        }
    }

    public void GiveTreesNutrients()
    {
        if (nutrientEvent != null)
        {
            nutrientEvent();
        }
    }

    public int GetCurrentNutrientValue()
    {
        int nutrientValue = gameObject.GetComponent<NutrientManager>().nutrientAmount;
        return nutrientValue;
    }

    public List<Transform> getTouchedTrees()
    {

        return touchedTrees;
    }

    public int GetCurrentTouchedTreeCount()
    {
        int treeCount = touchedTrees.Count;
        return treeCount;
    }

    public void AddedTree(Transform tree)
    {
        touchedTrees.Add(tree);
        _nutrientManager.NutrientLevelSplit();
        treeScoreText.text = "Trees: " + touchedTrees.Count;
        UpdateMusic(touchedTrees.Count);
    }

    void UpdateMusic(int treeNumber)
    {
        if (treeNumber == 1)
        {
            _audioManager.Play("1 tree");
        }

        if (treeNumber == 2)
        {
            _audioManager.Play("2 tree");
        }
        
        if (treeNumber == 3)
        {
            _audioManager.Play("3 tree");
        }
        
        if (treeNumber == 4)
        {
            _audioManager.Play("4 tree");
        }
        
        if (treeNumber == 5)
        {
            _audioManager.Play("5 tree");
        }

        if (treeNumber == 6)
        {
            _audioManager.Play("Background Music");
        }
    }
    
    public void PlayClick()
    {
        FindObjectOfType<AudioManager>().Play("Button");

    }

   
}

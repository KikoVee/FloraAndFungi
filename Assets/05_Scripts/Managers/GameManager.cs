using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    [Space] [Header("(Location and Managers)")]

    public static GameManager currentManager;
    public HexMapEditor _hexMapEditor;
    private NutrientManager _nutrientManager;
    private AudioManager _audioManager;
    
    [Space] [Header("(Scores)")]

    public CollectableAnimation _sugarCollectableAnimation;
    private int sugarScore;
    [SerializeField] private Text sugarScoreText;
    private int nutrientScore;
    [SerializeField] private Text nutrientScoreText;
    [SerializeField] private Text nutrientCostText;
    [SerializeField] private Text treeScoreText;
    [SerializeField] private GameObject timeLapsePauseImage;
    [SerializeField] private Text timeLapsePauseText;
    [SerializeField] private GameObject gameOverImage;

    private Color originalTextColor;

    public delegate void EndTurnEvent();         //when player ends the turn it calls all other onTurnEnd events from other scripts
    public EndTurnEvent onTurnEnd;
    bool timeLapseClicked = false;
    [SerializeField] private GameObject timeLapseImage;


   // public delegate void GiveNutrientsEvent();
   // public static GiveNutrientsEvent nutrientEvent;
    //public delegate void ExpansionEvent();
    //public static ExpansionEvent addExpansionEvent;

    public Transform fungiPrefab;
    public bool fungiAlive = true;


    public bool turnEndSequence;
    public bool timelapse = false;
    private float timer;
    private float time = 0.2f;
    public List<Transform> touchedTrees = new List<Transform>();
    public List<GameObject> fungi = new List<GameObject>();
    public List<TreeBehaviour> treesInScene = new List<TreeBehaviour>();

    public Image ecoResilienceImage;
    [SerializeField] private int completeTrees;
 
    
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
        EcosystemResilienceCheck();
        foreach (TreeBehaviour tree in treesInScene)
        {
            tree.NewCycle();
        }
        foreach (var fungus in fungi)
        {
            fungus.GetComponent<FungiBehaviour>().UpgradeEvent();
        }
        
        WeatherManager.currentWeatherManager.NewCycle();
        NutrientManager.currentNutrientManager.NewCycleSugar();
        
        
        //begins revalue cycle for all trees and skips time ahead quickly
        /*if (onTurnEnd != null)
        {
            onTurnEnd();
        }*/
        
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
        foreach (TreeBehaviour tree in treesInScene)
        {
            tree.GetNutrients();
        }
    }
    
    public void ExpandedNetwork()
    {
        foreach (TreeBehaviour tree in treesInScene)
        {
            tree.CheckNeighbors();
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
        _nutrientManager.NutrientLevelSplit(1);
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

    public void UnhealthyFungi(int number)
    {
        if (number == -fungi.Count)
        {
            fungiAlive = false;
            GameOver(); //no more fungi  == no more game
        }
        else
        {
            fungiAlive = true;
        }
        
        for (int i = 0; i > number; i--)
        {

            FungiBehaviour fungus = fungi[Random.Range(0, fungi.Count)].GetComponent<FungiBehaviour>();
            fungus.SetUnhealthy();
        }
    }

    public void HealthyFungi()
    {
        fungiAlive = true;
        foreach (var _fungus in fungi)
        {
            FungiBehaviour fungus = _fungus.GetComponent<FungiBehaviour>();
            fungus.SetHealthy();
        }
    }

    private void EcosystemResilienceCheck()
    {
        int numberComplete = 0;
        foreach (TreeBehaviour tree in treesInScene)
        {
            if (tree.treeState == TreeBehaviour.TreeState.complete)
            {
                numberComplete += 1;
            }
        }

        completeTrees = numberComplete;
        UpdateResilienceGraphic();
    }

    private void UpdateResilienceGraphic()
    {
        float percentComplete = ((float) completeTrees / (float) treesInScene.Count) * 100;
        ecoResilienceImage.fillAmount = percentComplete/100;
    }

    private void GameOver()
    {
        gameOverImage.SetActive(true);
        Debug.Log("game over");
    }

    public void Restart()
    {
        FindObjectOfType<GameSceneManager>().LoadGame();
    }

    public void ExitGame()
    {
        FindObjectOfType<GameSceneManager>().ExitGame();
    }
    

}

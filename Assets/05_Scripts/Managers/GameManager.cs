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
    public TutorialManager _tutorialManager;
    private NutrientManager _nutrientManager;
    private AudioManager _audioManager;
    
    [Space] [Header("(UI Elements)")]

    public CollectableAnimation _sugarCollectableAnimation;
    private int sugarScore;
    private int sugarBeforeTimelapse;
    [SerializeField] private Text sugarScoreText;
    private int nutrientScore;
    [SerializeField] private Text nutrientScoreText;
    [SerializeField] private Text nutrientCostText;
    [SerializeField] private Text treeScoreText;
    [SerializeField] private GameObject timeLapsePauseImage;
    [SerializeField] private GameObject timeLapsePlayImage;
    [SerializeField] private GameObject timeLapseImage;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject gameWinImage;
    [SerializeField] public Image ecoResilienceImage;
    [SerializeField] public GameObject menuImage;
    [SerializeField] public GameObject nextButton;
    bool timeLapseClicked = false;
    private Color originalTextColor;


    [Space] [Header("(Game Objects and Bools)")]

    public Transform fungiPrefab;
    public bool fungiAlive = true;


    public bool turnEndSequence;
    [SerializeField] private GameObject sun;
    public bool timelapse = false;
    private float timer;
    private float time = 3f; //time in inbetween sequence
    public List<Transform> touchedTrees = new List<Transform>();
    public List<GameObject> fungi = new List<GameObject>();
    public List<TreeBehaviour> treesInScene = new List<TreeBehaviour>();

    [SerializeField] private int completeTrees;

    public bool tutorial = true;
    public bool gameOver = false;
    public bool gameWin = false;
 
    
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
        StopMusic();
        treeScoreText.text = "Trees: " + touchedTrees.Count + "/" + treesInScene.Count;


    }

    private void Update()
    {
        if (turnEndSequence && timer > 0)
        {
            timer -= Time.deltaTime;
            sun.GetComponent<DayCycles>().ChangeGameSpeed(30f);
            //Debug.Log("timer is " + timer);
        }
        else
        {
            turnEndSequence = false;
            sun.GetComponent<DayCycles>().ChangeGameSpeed(.5f);
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
        _audioManager.Play("Spores");
        
        
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
        sugarBeforeTimelapse = sugarScore;
        GiveTreesNutrients();

        EndTurn();

        timelapse = true;
        turnEndSequence = true;
        timer = time;
        timeLapseClicked = true;
        timeLapsePlayImage.SetActive(false);
        timeLapsePauseImage.SetActive(true);
        timeLapseImage.SetActive(true);
        gameWinImage.SetActive(false);
        gameOverImage.SetActive(false);

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
            timeLapsePlayImage.SetActive(true);
            timeLapsePauseImage.SetActive(false);
            timeLapseImage.SetActive(false);
            if (gameWin)
            {
                gameWinImage.SetActive(true);
            }
            if (gameOver)
            {
                gameOverImage.SetActive(true);
            }

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
        _nutrientManager.NutrientLevelSplit();
        treeScoreText.text = "Trees: " + touchedTrees.Count + "/" + treesInScene.Count;
        UpdateMusic(touchedTrees.Count);
    }

    void StopMusic()
    {
        _audioManager.Stop("1 tree");
        _audioManager.Stop("2 tree");
        _audioManager.Stop("3 tree");
        _audioManager.Stop("4 tree");
        _audioManager.Stop("5 tree");
        _audioManager.Stop("Background Music");

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
            _audioManager.Pause("1 tree");

        }
    }
    
    public void PlayClick()
    {
        FindObjectOfType<AudioManager>().Play("Button");

    }

    public void UnhealthyFungi(int number)
    {
        if (_tutorialManager.firstHurtFungi != true)
        {
            _tutorialManager.LostFungi();   
        }
        if (number == -fungi.Count) //if not enough sugar to feed fungi count 
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
        FungiNutrientCount(number); 

    }

    public void HealthyFungi()
    {
        fungiAlive = true;
        foreach (var _fungus in fungi)
        {
            FungiBehaviour fungus = _fungus.GetComponent<FungiBehaviour>();
            fungus.SetHealthy();
        }
        FungiNutrientCount(0);
    }

    private void FungiNutrientCount(int count)
    {
        int healthyCount = fungi.Count + count;
        _nutrientManager.HealthyFungiCount(healthyCount);
        
    }
    
    public void AddFungiToList(GameObject fungus)
    {
       fungi.Add(fungus);
       _nutrientManager.UpdateSugarNeededText();
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
        if (completeTrees == treesInScene.Count)
        {
            GameWin();
        }
    }

    private void UpdateResilienceGraphic()
    {
        float percentComplete = ((float) completeTrees / (float) treesInScene.Count) * 100;
        ecoResilienceImage.fillAmount = percentComplete/100;
    }

    private void GameWin()
    {
        gameWinImage.SetActive(true);
        gameWin = true;

    }
    
    private void GameOver()
    {
        foreach (var tree in treesInScene)
        {
            tree.GetComponent<TreeBehaviour>().ChangeMushroomVisual();
        }

        gameOver = true;
        gameOverImage.SetActive(true);
        nextButton.SetActive(false);
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
    public void ReturnToMainMenu()
    {
        FindObjectOfType<GameSceneManager>().LoadMenu();
    }

    public void MenuPopup()
    {
        if (!menuImage.activeSelf)
        {
            menuImage.SetActive(true);
        }
        else
        {
            menuImage.SetActive(false);
        }
    }

    public void FirstClick()
    {
        if (_tutorialManager != null && _tutorialManager.firstClick == false)
        {
            _tutorialManager.firstClick = true;
            Debug.Log("first click");
        }
    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject weatherChangeExplanation;
    private Image weatherExplanationImage;
    private Text weatherExplanationText;
    [SerializeField] private GameObject nutrientText;
    [SerializeField] private GameObject sugarText;
    [SerializeField] private GameObject treeText;
    [SerializeField] private GameObject NextButton;




    [SerializeField] private GameObject[] tutorialPopups;
    [SerializeField] private GameObject TutorialQuestion;
    public GameObject controlsPopup;
    
    
    [SerializeField] private Color myImageColor;
    [SerializeField] private Color myTextColor;
    public float fadeTime;
    public bool displayInfo;
    private float timer;
    [SerializeField] private float displayTime = 6;
    
    public bool tutorial;
    public bool firstClick = false;
    public bool collectedSugar = false;
    public bool firstHurtFungi = false;
    private float tutorialTimer = 5f;
    

    
    
    // Start is called before the first frame update
    void Start()
    {
        TutorialQuestion.SetActive(true);
        weatherChangeExplanation.SetActive(true);
        weatherExplanationImage = weatherChangeExplanation.GetComponentInChildren<Image>();
        weatherExplanationText = weatherChangeExplanation.GetComponentInChildren<Text>();

        weatherExplanationImage.color = Color.clear;
        weatherExplanationText.color = Color.clear;
        
    }

    public void YesTutorial()
    {
        tutorial = true;
        if (tutorial)
        {
            tutorialPopups[0].SetActive(true);
            NextButton.SetActive(false);
            weatherChangeExplanation.SetActive(false);
        }
        TutorialQuestion.SetActive(false);
    }

    public void NoTutorial()
    {
        tutorial = false;
        TutorialQuestion.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (timer > 0)
        {
            displayInfo = true;
            timer -= Time.deltaTime;
        }
        else
        {
            displayInfo = false;
        }
        
        FadeText();

        if (tutorial)
        {
            TutorialMode();
        }
        
    }

    public void WasRaining()
    {
        timer = displayTime;
        weatherExplanationText.text = "The rain seems to have helped the trees.";
    }
    
    public void WasDry()
    {
        timer = displayTime;
        weatherExplanationText.text = "The drought damaged the trees.";
    }

    public void LostFungi()
    {
        timer = displayTime;
        weatherExplanationText.text = "Not enough sugar to feed fungi.";
        firstHurtFungi = true;
    }

    private void FadeText()
    {
        if (displayInfo)
        {
            weatherExplanationImage.color = Color.Lerp(weatherExplanationImage.color, myImageColor, fadeTime * Time.deltaTime);
            weatherExplanationText.color = Color.Lerp(weatherExplanationText.color, myTextColor, fadeTime * Time.deltaTime);
        }
        else
        {
            weatherExplanationText.color = Color.Lerp(weatherExplanationText.color, Color.clear, fadeTime * Time.deltaTime);
            weatherExplanationImage.color = Color.Lerp(weatherExplanationImage.color, Color.clear, fadeTime * Time.deltaTime);
        }
       
    }

    public void TutorialMode()
    {
        if (tutorial)
        {
            if (tutorialPopups[0].activeSelf)
            {
                if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
                {
                    tutorialPopups[0].SetActive(false);
                    tutorialPopups[1].SetActive(true);
                } 
            }
            
            if (tutorialPopups[1].activeSelf && GameManager.currentManager.fungi.Count == 1)
            {
                tutorialPopups[1].SetActive(false);
                sugarText.SetActive(false); // toggles the ui elements on and off
                tutorialPopups[2].SetActive(true);
            }

            if (tutorialPopups[2].activeSelf && GameManager.currentManager.fungi.Count >= 2)
            {
                tutorialPopups[2].SetActive(false);
                sugarText.SetActive(true);
                tutorialPopups[3].SetActive(true);
            }

            if (tutorialPopups[3].activeSelf && GameManager.currentManager.touchedTrees.Count >= 1 && GameManager.currentManager.fungi.Count >= 3)
            {
                NextButton.SetActive(true);
                tutorialPopups[3].SetActive(false);
                tutorialPopups[4].SetActive(true);
            }

            if (tutorialPopups[4].activeSelf && firstClick)
            {
                tutorialPopups[4].SetActive(false);
                tutorialPopups[5].SetActive(true);
                tutorialPopups[6].SetActive(true);
            }
            if (tutorialPopups[5].activeSelf && collectedSugar)
            {
                tutorialPopups[5].SetActive(false);
            }
            
            if (tutorialPopups[6].activeSelf && GameManager.currentManager.touchedTrees.Count >= 2)
            {
                tutorialPopups[6].SetActive(false);
                tutorialPopups[7].SetActive(true);
                weatherChangeExplanation.SetActive(true);
            }

            if (tutorialPopups[7].activeSelf)
            {
                tutorialTimer -= Time.deltaTime;

                if (tutorialTimer <= 0)
                {
                    TutorialEnd();
                }
            }
        }
        
    }

    public void TutorialEnd()
    {
        foreach (var popUp in tutorialPopups)
        {
            popUp.SetActive(false);
        }

        tutorial = false;
        weatherChangeExplanation.SetActive(true);
        NextButton.SetActive(true);

        
        
    }
    
}

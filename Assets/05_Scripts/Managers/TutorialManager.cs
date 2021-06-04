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
    
    
    [SerializeField] private Color myImageColor;
    [SerializeField] private Color myTextColor;
    public float fadeTime;
    public bool displayInfo;
    private float timer;
    [SerializeField] private float displayTime = 6;
    public bool tutorial;
    public bool firstClick = false;
    public bool collectedSugar = false;
    private float tutorialTimer = 5f;

    
    
    // Start is called before the first frame update
    void Start()
    {
        tutorial = true;
        weatherExplanationImage = weatherChangeExplanation.GetComponentInChildren<Image>();
        weatherExplanationText = weatherChangeExplanation.GetComponentInChildren<Text>();

        weatherExplanationImage.color = Color.clear;
        weatherExplanationText.color = Color.clear;
        
        if (tutorial)
        {
            tutorialPopups[0].SetActive(true);
            NextButton.SetActive(false);
            weatherChangeExplanation.SetActive(false);
        }
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
            if (tutorialPopups[0].activeSelf && GameManager.currentManager.fungi.Count == 1)
            {
                tutorialPopups[0].SetActive(false);
                sugarText.SetActive(false); // toggles the ui elements on and off
                tutorialPopups[1].SetActive(true);
            }

            if (tutorialPopups[1].activeSelf && GameManager.currentManager.fungi.Count >= 3)
            {
                tutorialPopups[1].SetActive(false);
                sugarText.SetActive(true);
                tutorialPopups[2].SetActive(true);
            }

            if (tutorialPopups[2].activeSelf && GameManager.currentManager.touchedTrees.Count >= 1)
            {
                NextButton.SetActive(true);
                tutorialPopups[2].SetActive(false);
                tutorialPopups[3].SetActive(true);
            }

            if (tutorialPopups[3].activeSelf && firstClick)
            {
                tutorialPopups[3].SetActive(false);
                tutorialPopups[4].SetActive(true);
                tutorialPopups[5].SetActive(true);

            }
            if (tutorialPopups[4].activeSelf && collectedSugar)
            {
                tutorialPopups[4].SetActive(false);
            }
            
            if (tutorialPopups[5].activeSelf && GameManager.currentManager.touchedTrees.Count >= 2)
            {
                tutorialPopups[5].SetActive(false);
                tutorialPopups[6].SetActive(true);
            }

            if (tutorialPopups[6].activeSelf)
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

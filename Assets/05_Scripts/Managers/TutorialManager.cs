using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject weatherChangeExplanation;
    private Image weatherExplanationImage;
    private Text weatherExplanationText;
    
    [SerializeField] private Color myImageColor;
    [SerializeField] private Color myTextColor;
    public float fadeTime;
    public bool displayInfo;
    private float timer;
    [SerializeField] private float displayTime = 6;
    
    // Start is called before the first frame update
    void Start()
    {
        weatherExplanationImage = weatherChangeExplanation.GetComponentInChildren<Image>();
        weatherExplanationText = weatherChangeExplanation.GetComponentInChildren<Text>();

        weatherExplanationImage.color = Color.clear;
        weatherExplanationText.color = Color.clear;
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
}

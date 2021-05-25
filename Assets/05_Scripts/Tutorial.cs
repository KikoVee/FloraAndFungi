using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] public GameObject[] tutorialText;
    private GameObject text;
    private Text oldText;
    private int textNumber = 0;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject playButton;

        
    // Start is called before the first frame update
    void Start()
    {
        text = tutorialText[0];
        text.SetActive(true);
        continueButton.SetActive(true);
        playButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
         
        
    }

    public void TutorialText()
    {
        if (textNumber < tutorialText.Length - 1)
        {
            GameObject oldText = text;
            textNumber += 1;
            text = tutorialText[textNumber];
            text.SetActive(true); 
            oldText.SetActive(false);  
        }
        else
        {
            continueButton.SetActive(false);
            playButton.SetActive(true);
        }
        
    }
}

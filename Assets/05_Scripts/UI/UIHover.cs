using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public GameObject textPrefab;
    private Text myText;
    private Image myImage;
    [SerializeField] private Color myImageColor;
    [SerializeField] private Color myTextColor;

    public float fadeTime;
    public bool displayInfo;
    
    
    void Start()
    {
        myImage = textPrefab.GetComponentInChildren<Image>();
        myImage.color = Color.clear;
        myText = textPrefab.GetComponentInChildren<Text>();
        myText.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        FadeText();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        displayInfo = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        displayInfo = false;

    }

    

    private void FadeText()
    {
        if (displayInfo)
        {
            myImage.color = Color.Lerp(myImage.color, myImageColor, fadeTime * Time.deltaTime);
            myText.color = Color.Lerp(myText.color, myTextColor, fadeTime * Time.deltaTime);
        }
        else
        {
            myText.color = Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);
            myImage.color = Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);
        }
        
    }
}

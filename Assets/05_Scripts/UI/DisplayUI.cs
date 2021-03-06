using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUI : MonoBehaviour
{
    public string myString;
    public Text textPrefab;
    public Text myText;
    public float fadeTime;
    public bool displayInfo;
    private Camera camera;
    private Canvas myCanvas;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        myCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        camera = Camera.main;
        myText = Instantiate(textPrefab, myCanvas.transform).GetComponent<Text>();
        myText.color = Color.clear;    
    }

    

    // Update is called once per frame
    void Update()
    {
        myText.transform.position = camera.WorldToScreenPoint(transform.position);
        FadeText();
        
    }

    void OnMouseOver()
    {
         
        displayInfo = true;
    }

    private void OnMouseExit()
    {
        displayInfo = false;
    }

    

    private void FadeText()
    {
        if (displayInfo)
        {
            //myText.text = myString;
            myText.color = Color.Lerp(myText.color, Color.white, fadeTime * Time.deltaTime);
        }
        else
        {
            myText.color = Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);
        }
        
    }
}

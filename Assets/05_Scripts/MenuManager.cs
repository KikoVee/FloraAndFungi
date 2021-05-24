using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private ParticleSystem spores;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TutorialUpgrade()
    {
        continueButton.SetActive(true);         
        upgradeButton.SetActive(false); 
        spores.Play();
    }

    public void NextScene()
    {
        
    }
}

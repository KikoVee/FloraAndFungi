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
    [SerializeField] private TreeMainMenu[] trees;
    
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Background Music");

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

        foreach (TreeMainMenu tree in trees)
        {
           tree.MainMenuEffect(); 
        }
        
    }

    public void PlayGame()
    {
        FindObjectOfType<GameSceneManager>().LoadIntro();
    }

    public void PlayClick()
    {
        FindObjectOfType<AudioManager>().Play("Button");

    }
}

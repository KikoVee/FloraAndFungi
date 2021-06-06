using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //[SerializeField] private GameObject upgradeButton;
    //[SerializeField] private GameObject continueButton;
    //[SerializeField] private ParticleSystem spores;
    //[SerializeField] private TreeMainMenu[] trees;

    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject info;
    [SerializeField] private GameObject menu;

    
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Background Music");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   /* public void TutorialUpgrade()
    {
        continueButton.SetActive(true);         
        upgradeButton.SetActive(false); 
        spores.Play();

        foreach (TreeMainMenu tree in trees)
        {
           tree.MainMenuEffect(); 
        }
        
    }*/

    public void PlayGame()
    {
        FindObjectOfType<GameSceneManager>().LoadIntro();
    }

    public void PlayClick()
    {
        FindObjectOfType<AudioManager>().Play("Button");
    }

    public void CreditsEnabled()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }
    public void InfoEnabled()
    {
        menu.SetActive(false);
        info.SetActive(true);
    }
    public void CreditsDisabled()
    {
        credits.SetActive(false);
        info.SetActive(false);
        menu.SetActive(true);
    }

    public void QuitGame()
    {
        FindObjectOfType<GameSceneManager>().ExitGame();
    }

    public void OpenURL()
    {
        Application.OpenURL("https://www.sciencemag.org/news/2020/08/hidden-webs-fungi-protect-some-forests-drought-leave-others-vulnerable");
    }
}

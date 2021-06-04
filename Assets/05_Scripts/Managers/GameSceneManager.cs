using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }


    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void LoadIntro()
    {
        SceneManager.LoadScene(1);
    }
    
    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }
    
    public void ExitGame()
    {
      Application.Quit();  
    }

    
}

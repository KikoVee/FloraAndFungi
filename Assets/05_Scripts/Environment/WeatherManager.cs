using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WeatherManager : MonoBehaviour
{
    private GameManager _currentManager;
    public float weatherValue;
    public static WeatherManager currentWeatherManager;
    public GameObject dayCycles;
    public ParticleSystem rainParticles;
    public ParticleSystem rainDrops;
    public Volume postProcessing;
    private VolumeProfile _volumeProfile;
    private int speed = 1;

    public float weatherMin = -5;
    public float weatherMax = 10;
    private float lastWeather = 0;
    

    private void Awake()
    {
        if (currentWeatherManager == null)
        {
            currentWeatherManager = this;
        }
        else
        {
            Destroy(this);
        }
        
    }
    void Start()
    {
        _currentManager = GameManager.currentManager;
        GameManager.onTurnEnd += NewCycle;
        _volumeProfile = postProcessing.sharedProfile;
        ChangeState();


    }

    // Update is called once per frame
    void Update()
    {
        /*if (_currentManager.turnEndSequence)
        {
            dayCycles.SetActive(true);
        }
        else
        {
            dayCycles.SetActive(false);
        }*/

        if (weatherValue >= 0)
        {
            if (postProcessing.weight <= 1)
            {
                postProcessing.weight += Time.deltaTime;  
            }
           
        }
        else
        {
            if (postProcessing.weight >= 0)
            {
                postProcessing.weight -= Time.deltaTime;  
            }            
            
            

        }
    }

    void NewCycle()
    {
        float newWeather = Random.Range(weatherMin, weatherMax);
        
        if (lastWeather < 0) //checks if multiple days in a row of drought
        {
            newWeather += -5f;
        }
        
        weatherValue = Mathf.Clamp(newWeather, -10, weatherMax);
        lastWeather = weatherValue;
        ChangeState();
        
 
    }

    void ChangeState()
    {
        if (weatherValue >= 0)
        {
           
            rainParticles.Play();
            rainDrops.Play();
            FindObjectOfType<AudioManager>().Play("Rain");
        }
        else
        {
                          
            rainParticles.Stop();
            rainDrops.Stop();
            FindObjectOfType<AudioManager>().Pause("Rain");

        }
    }

    
    
}

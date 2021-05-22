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

        if (weatherValue >= 70)
        {
            if (postProcessing.weight <= 1)
            {
                postProcessing.weight += Time.deltaTime;  
            }
            
            rainParticles.Play();
            rainDrops.Play();
        }
        else
        {
            if (postProcessing.weight >= 0)
            {
                postProcessing.weight -= Time.deltaTime;  
            }            
            
            rainParticles.Stop();
            rainDrops.Stop();
        }
    }

    void NewCycle()
    {
        float newWeather = Random.Range(0,100);
        weatherValue = newWeather;
        
        
        
    }

    
    
}

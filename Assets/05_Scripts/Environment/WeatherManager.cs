using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    private GameManager _currentManager;
    public float weatherValue;
    public static WeatherManager currentWeatherManager;
    public GameObject dayCycles;
    public ParticleSystem rainParticles;
    public ParticleSystem rainDrops;


    

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
            rainParticles.Play();
            rainDrops.Play();
        }
        else
        {
            rainParticles.Stop();
            rainDrops.Stop();
        }
    }

    void NewCycle()
    {
        float newWeather = Random.Range(0,100);
        weatherValue = newWeather;
        
    }

    void WeatherCycle()
    {
        
    }
    
}

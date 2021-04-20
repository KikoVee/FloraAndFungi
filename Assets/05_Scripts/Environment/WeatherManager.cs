using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    private GameManager _currentManager;
    public float weatherValue;
    public static WeatherManager currentWeatherManager;
    

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
        
    }

    void NewCycle()
    {
        float newWeather = Random.Range(0,100);
        weatherValue = newWeather;
    }
}

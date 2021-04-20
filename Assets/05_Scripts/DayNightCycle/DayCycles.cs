using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycles : MonoBehaviour
{
    [Header("Time")]
    [Tooltip("Day Length in Minutes")]
    [SerializeField]
    private float _targetDayLength = 0.5f; //length day in min
    public float targetDayLength //public access to set day length
    {
        get
        {
            return _targetDayLength;
        }
    }
    private float _rawDayNumber;
    public float rawDayNumber
    {
        get
        {
            return _rawDayNumber;
        }
    }


    [SerializeField]
    [Range(0f, 1f)]
    private float _timeOfDay;
    public float timeOfDay
    {
        get
        {
            return _timeOfDay;

        }

    }

    [SerializeField]
    private int _dayNumber = 0; //tracks days passed

   

    public int dayNumber // need script to track days passed
    {
        get
        {
            return _dayNumber;
        }
    }

    [SerializeField]
    private int _yearNumber = 0;
    public int yearNumber
    {
        get
        {
            return _yearNumber;
        }
    }
    private float _rawYearNumber;
    public float rawYearNumber
    {
        get
        {
            return _rawYearNumber;
        }
    }

    private float _timeScale = 100f;

    public float theTimeScale
    {
        get { return _timeScale; }
    }
    [SerializeField]
    private int _yearLength = 100; //number days in year
    public int yearLength
    {
        get
        { return _yearLength;
        }
    }

    public bool pause = false; //pause day/night cycle without pausing game for debug

    [SerializeField]
    [Range (0f,100f)]
    private float gameSpeed; //24 is nice

    [Header("Sun Light")]
    [SerializeField]
    private Transform dailyRotation;
    [SerializeField]
    private Light sun;
    public float intensity;
    [SerializeField]
    private float sunBaseIntensity = 1f;
    [SerializeField]
    private float sunVariation = 1.5f;
    [SerializeField]
    private Gradient sunColor;
    [SerializeField]
    private Material skyMaterial;


    private void Start()
    {
        //RenderSettings.skybox = skyMaterial;
    }

    private void Update()
    {
        if (!pause)
        {
            UpdateTimeScale();
            UpdateTime();
        }

        AdjustSunRotation();
        SunIntensity();
        AdjustSunColor();
    }
    private void UpdateTimeScale()
    {
        _timeScale = gameSpeed / (_targetDayLength / 360); //time needs to be 24x faster in game if a 24h ingame = 1hr real life. Was /60
    }

    public void UpdateTime()
    {
       

        _timeOfDay += Time.deltaTime * _timeScale / 86400; //second in a day
        if(_timeOfDay > 1) // new day
        {
            _dayNumber++;
            _timeOfDay -= 1;

            if(_dayNumber >= _yearLength) //new Year!
            {
                _yearNumber++;
                _dayNumber = 0;
               // GameManager.currentManager.NewYear();
            }
        }

        _rawDayNumber = _timeOfDay + _dayNumber;
        _rawYearNumber = (_rawDayNumber / _yearLength) + yearNumber; //calculates the exact time during the year
        //Debug.Log("raw year number is " + _rawYearNumber);
    }

    private void AdjustSunRotation() //rotates sun daily 
    {
        float sunAngle = timeOfDay * 360f;
        dailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, sunAngle));
    }

    private void SunIntensity()
    {
        intensity = Vector3.Dot(sun.transform.forward, Vector3.down);
        intensity = Mathf.Clamp01(intensity);

        sun.intensity = intensity * sunVariation + sunBaseIntensity;
    }

    private void AdjustSunColor()
    {
        sun.color = sunColor.Evaluate(intensity); //sets color of sun based on height
        //skyMaterial.color = sunColor.Evaluate(intensity);
        //skyMaterial.color = Color.red;
        if (RenderSettings.skybox.HasProperty("_Tint"))
            RenderSettings.skybox.SetColor("_Tint", sun.color);
        else if (RenderSettings.skybox.HasProperty("_SkyTint"))
            RenderSettings.skybox.SetColor("_SkyTint", Color.yellow);

    }

    public float GetCurrentIntensity()
    {
        return intensity; 
    }

    public int GetCurrentYearNumber()
    {
        return yearNumber;
    }

    public void ChangeGameSpeed(float newSpeed)
    {
        gameSpeed = newSpeed;
    }
}

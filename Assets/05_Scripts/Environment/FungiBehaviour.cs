using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiBehaviour : MonoBehaviour
{
    private NutrientManager _nutrientManager;
    public ParticleSystem spores;
    
    // Start is called before the first frame update
    void Start()
    {
        _nutrientManager = NutrientManager.currentNutrientManager;
        NutrientManager.addNutrientEvent = UpgradeEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpgradeEvent()
    {
        spores.Play();
    }
}

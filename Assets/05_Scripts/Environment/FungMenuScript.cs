using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungMenuScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem spores;
    
    // Start is called before the first frame update
    void Start()
    {
        NutrientManager.addNutrientEvent += UpgradeEvent;
    }

   

    public void UpgradeEvent()
    {
        spores.Play(); 
    }
}

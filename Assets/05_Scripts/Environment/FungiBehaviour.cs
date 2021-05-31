using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiBehaviour : MonoBehaviour
{
    [SerializeField] private ParticleSystem spores;
    
    // Start is called before the first frame update
    void Start()
    {
        NutrientManager.addNutrientEvent += UpgradeEvent;
        PlayStartSound();
    }

   

    public void UpgradeEvent()
    {
        spores.Play(); 
    }

    private void PlayStartSound()
    {

        int randomChime = Random.Range(0, 2);

        if (randomChime == 0)
        {
            FindObjectOfType<AudioManager>().Play("Pop");
        }
        if (randomChime == 1)
        {
            FindObjectOfType<AudioManager>().Play("Pop 2");
        }
       
       

    }
}

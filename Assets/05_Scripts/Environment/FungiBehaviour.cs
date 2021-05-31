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
        FindObjectOfType<AudioManager>().Play("Pop");

        /*int randomChime = Random.Range(0, 4);

        if (randomChime == 0)
        {
            FindObjectOfType<AudioManager>().Play("Piano Chime 1");
        }
        if (randomChime == 1)
        {
            FindObjectOfType<AudioManager>().Play("Piano Chime 2");
        }
        if (randomChime == 2)
        {
            FindObjectOfType<AudioManager>().Play("Piano Chime 3");
        }
        if (randomChime == 3)
        {
            FindObjectOfType<AudioManager>().Play("Piano Chime 4");
        }*/

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiBehaviour : MonoBehaviour
{
    [SerializeField] private ParticleSystem spores;
    [SerializeField] private GameObject[] mushroomPrefabs;
    [SerializeField] private Material healthyMushroom;
    [SerializeField] private Material unhealthyMushroom;
    public enum FungiState {healthy, unhealthy};

    public FungiState fungiState;

    private int mushroomVisual;
    
    
    // Start is called before the first frame update
    void Start()
    {
       // GameManager.onTurnEnd += UpgradeEvent;
        PlayStartSound();
        PickVisual();
    }

   

    public void UpgradeEvent()
    {
        if (spores != null)
        {
            spores.Play(); 
        }
        
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

    private void PickVisual()
    {
        mushroomVisual = Random.Range(0, mushroomPrefabs.Length);
        
        mushroomPrefabs[mushroomVisual].SetActive(true);
    }

    public void SetUnhealthy()
    {
        //Debug.Log("set " + gameObject + " unhealthy");
        Renderer[] childrenRenderers = mushroomPrefabs[mushroomVisual].GetComponentsInChildren<Renderer>();
       
        foreach (Renderer renderer in childrenRenderers)
        {
            renderer.material = unhealthyMushroom;
        }

        fungiState = FungiState.unhealthy;
    }

    public void SetHealthy()
    {
        //Debug.Log("set " + gameObject + " healthy");
        Renderer[] childrenRenderers = mushroomPrefabs[mushroomVisual].GetComponentsInChildren<Renderer>();
       
        foreach (Renderer renderer in childrenRenderers)
        {
            renderer.material = healthyMushroom;
        }
        fungiState = FungiState.unhealthy;

    }
    
    
}

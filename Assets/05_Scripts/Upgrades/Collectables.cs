using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Collectables : MonoBehaviour
{
    public int pointValue;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

    }

    public int GetValue(int amount)
    {
        amount = pointValue;
        return amount; 
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShaderAnimation : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
  //  [SerializeField] private Material shaderMaterial;
    
    

    public float targetDissolveValue = 1;
    private float currentDissolveValue = -1;
    
    
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        // shaderMaterial = gameObject.GetComponent<Material>();
    }

    void Update()
    {
       

    }

    public void AnimateShader(Vector3 center)
    {
            renderer.material.SetVector("_RipplePosition", center);
            currentDissolveValue = Mathf.Lerp(currentDissolveValue, targetDissolveValue, Time.deltaTime);
            //float dissolveValue = Mathf.PingPong(Time.deltaTime, 1);
            renderer.material.SetFloat("_RippleStartTime", currentDissolveValue); 
        
       
    }

    private void HandleHealthChange(int health, int maxHealth)
    {
        targetDissolveValue = (float) health / maxHealth;
    }
}

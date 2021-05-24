using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMainMenu : MonoBehaviour
{
    private int maxTreeHealth = 100;
    [SerializeField] private float currentTreeHealth;


    public Material[] treeMaterial;
    public Material[] TreeLeavesMaterials;
    [SerializeField] private Renderer treeRenderer;
    public GameObject treeLeaves;
    
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private float newBlendValue = 0;
    private float oldBlendValue = 0;
    private float blendSpeed = 3;


    private void Start()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMesh = _skinnedMeshRenderer.sharedMesh;
        newBlendValue = oldBlendValue;
        TreeVisualChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (oldBlendValue != newBlendValue)
        {
            _skinnedMeshRenderer.SetBlendShapeWeight(0, oldBlendValue);
            oldBlendValue = Mathf.Lerp(oldBlendValue, newBlendValue, blendSpeed * Time.deltaTime);
        }
        else
        {
            oldBlendValue = newBlendValue;
        }
    }
    
    public void MainMenuEffect()
    {
        currentTreeHealth = 100f;
        TreeVisualChange();
    }

    void TreeVisualChange()
    {
        float healthPercent = currentTreeHealth;
        Renderer[] rend = treeLeaves.GetComponentsInChildren<Renderer>();
        if (healthPercent >= 90)
        {
            treeRenderer.material = treeMaterial[0];
            newBlendValue = 0;
            treeLeaves.SetActive(true);
            

        }

        if (healthPercent >= 50 && healthPercent <= 89)
        {
            treeRenderer.material = treeMaterial[1];
            newBlendValue = 30;
            treeLeaves.SetActive(true);
            foreach (Renderer renderer in rend)
            {
                renderer.materials[0] = TreeLeavesMaterials[0];

            }

        }

        if (healthPercent >= 11 && healthPercent <= 49)
        {
            treeRenderer.material = treeMaterial[2];
            newBlendValue = 60;
            treeLeaves.SetActive(false);
        }

        if (healthPercent <= 10)
        {
            treeRenderer.material = treeMaterial[3];
            newBlendValue = 100;
            treeLeaves.SetActive(false);



        }
    }
    
}

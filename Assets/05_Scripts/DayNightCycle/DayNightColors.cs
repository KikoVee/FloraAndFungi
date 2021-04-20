using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightColors : MonoBehaviour
{
    public Material objectMaterial;
    [SerializeField]
    private float colorIntensity;
    private GameObject sun;
    private GameObject thisObject;
    private DayCycles dayCycle;

    public Gradient objectColor;


    // Start is called before the first frame update
    void Start()
    {
        //thisObject = this.gameObject;
        //objectMaterial = GetComponent<Material>();
        sun = GameObject.FindGameObjectWithTag("sun");
        dayCycle = sun.GetComponent<DayCycles>(); //get the script from the sun
    }
    // Update is called once per frame
    void Update()
    {
        //dayCycle.intensity = colorIntensity;
        colorIntensity = dayCycle.intensity;
        AdjustObjectColor();

    }

    private void AdjustObjectColor()
    {
        objectMaterial.color = objectColor.Evaluate(colorIntensity); //sets color of sun based on height
        //objectMaterial.colorshaded = objectColor.Evaluate(colorIntensity);
    }
}

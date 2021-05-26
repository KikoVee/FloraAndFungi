using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    private Transform target;
    private Camera camera;
    
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Camera>().transform;
        camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(target);
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}

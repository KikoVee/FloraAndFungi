using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeScript : MonoBehaviour
{
   
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private float blendShape = 100f;

    public int growSpeed = 9;
    // Start is called before the first frame update
    void Start()
    {
        _skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blendShape > 0)
        {
            _skinnedMeshRenderer.SetBlendShapeWeight(0, blendShape);
            blendShape -= growSpeed * Time.deltaTime;
        }
    }
}

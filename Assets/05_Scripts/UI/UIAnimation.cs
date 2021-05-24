using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    private Sprite sprite;

    private Vector3 minScale;
    public Vector3 maxScale;
    public bool repeatable;
    [SerializeField] private float speed = 2f;

    [SerializeField] private float duration = 5f;
    // Start is called before the first frame update
    

    IEnumerator Start()
    {
        sprite = gameObject.GetComponent<Sprite>();
        minScale = gameObject.transform.localScale;

        while (repeatable)
        {
            yield return RepeatLerp(minScale, maxScale, duration);
            yield return RepeatLerp(maxScale, minScale, duration);
        }
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }
}

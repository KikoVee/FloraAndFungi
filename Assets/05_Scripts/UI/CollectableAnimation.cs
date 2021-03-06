using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectableAnimation : MonoBehaviour
{
    //[SerializeField] private Text targetText;
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private Transform target;
    private int _collectables;
    private Canvas myCanvas;
    private Camera camera;

    [Space] [Header("Available collectables : (collectables to pool)")] [SerializeField]
    private int maxCollectables;
    Queue<GameObject> collectablesQueue = new Queue<GameObject>();

    [Space] [Header("Animation Setting)")] 
    [SerializeField] [Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField] [Range(0.9f, 2.0f)] float maxAnimDuration;
    [SerializeField] private Ease easeType;
    [SerializeField] private Ease scaleType;


    private Vector3 targetPos;

    
    public int Collectables
    {
        get { return _collectables; }
        set { _collectables = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        myCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        camera = Camera.main;
        targetPos = target.position;
        PrepareCollectables();
    }

    void PrepareCollectables()
    {
        GameObject collectable;
        for (int i = 0; i < maxCollectables; i++)
        {
            collectable = Instantiate(collectablePrefab);
            collectable.transform.parent = myCanvas.transform;
            collectable.SetActive(false);
            collectablesQueue.Enqueue(collectable);  
        }    
    }

    void Animate(Vector3 collectablePosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (collectablesQueue.Count > 0)
            {
                GameObject collectable = collectablesQueue.Dequeue();
                collectable.SetActive(true);
                collectable.transform.position = collectablePosition;
                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                
                collectable.transform.DOMove(targetPos, duration)
                    .SetEase(easeType)
                    .OnComplete(() =>
                {
                    collectable.SetActive(false);
                    collectablesQueue.Enqueue(collectable);
                    Collectables++;
                });

                float scale = Random.Range(0.5f, 1f);
                collectable.transform.DOScale(collectable.transform.localScale, scale).SetEase(scaleType);
            }
        }
    }

    public void AddCollectable(Vector3 collectablePosition, int amount)
    {
        Vector3 position = camera.WorldToScreenPoint(collectablePosition);

        Animate(position, amount);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnimator : MonoBehaviour
{
    [SerializeField] public ParticleSystem particles;
    private bool play;
    
    // Start is called before the first frame update
    void Start()
    {
        //GameManager.onTurnEnd += NewCycle; 
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            float time = 3f;
            time -= Time.deltaTime;
            if (time >= 0)
            {
                particles.Play();
            }
            else
            {
                particles.Stop();
                play = false;
            }
        }
       
    }

    void NewCycle()
    {
        play = true;
    }
}

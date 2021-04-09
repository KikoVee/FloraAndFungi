using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementReplay : MonoBehaviour
{
    private List<Transform> thisWayPointList;

    private NavMeshAgent agent;
    private int destPoint = 0; 
    public bool playRecording = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playRecording)
            
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPoint();
            }
        }
    }

    public void BeginReplay()
    {
        Debug.Log("begin replay");
        thisWayPointList = gameObject.GetComponent<PlayerBehaviour>().wayPointList;
        gameObject.GetComponent<InputHandler>().recording = true;
       // gameObject.GetComponent<InputHandler>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        GoToNextPoint();
        playRecording = true;
    }
     
    void GoToNextPoint()
    {
        if (thisWayPointList.Count == 0)
        {
            return;   
        }

        agent.destination = thisWayPointList[destPoint].position;
        destPoint = (destPoint + 1) % thisWayPointList.Count;
        //Debug.Log("destination" + destPoint);
    }
}

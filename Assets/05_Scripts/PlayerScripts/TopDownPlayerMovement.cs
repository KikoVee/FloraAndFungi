using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TopDownPlayerMovement : MonoBehaviour
{
    private InputHandler _input;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    
    [SerializeField] private Camera camera;

    [SerializeField] private ParticleSystem mushroomParticles;
    [SerializeField] private ParticleSystem debriParticles;
    private NavMeshAgent agent;
    
    public bool letPlay = false;
    public bool navMeshWalking = false;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        float inputX = _input.InputVector.x;
        float inputY = _input.InputVector.y;
        //move in direction aiming
       var movementVector = MoveTowardTarget(targetVector);

        //rotate in direction aiming
        RotateTowardMovementDirection(movementVector);

        if (agent.enabled && agent.isStopped != true)
        {
            navMeshWalking = true;
        }
        else
        {
            navMeshWalking = false;
        }
        
        if(inputX != 0 || inputY != 0 || navMeshWalking)
        {
            letPlay = true;
        }
        else
        {
            letPlay = false;
        }
     
        if(letPlay)
        {
            if(!mushroomParticles.isPlaying)
            {
                mushroomParticles.Play();
            }
            
        }else{
            if(mushroomParticles.isPlaying)
            {
                mushroomParticles.Pause();
            }
        }

        
    }

    private void RotateTowardMovementDirection(Vector3 movementVector)
    {
        if(movementVector.magnitude == 0){return;} //keeps rotation of player 
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }
}

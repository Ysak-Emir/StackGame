using System;
using System.Collections.Generic;
using Scritps;
using UnityEngine;
using Random = System.Random;

public class MoveCube : MonoBehaviour
{
    private float _speedX;
    private float _speedY;
    private float _speedZ;
    private Vector2 _directionX;
    private Vector2 _directionY;
    private Vector3 _directionZ;
    
    
    // public MoveDirection currentMoving;
    // public Queue<MoveDirection> moveSequence;
    
    public bool _isMovingX = false;
    
    public bool _isMovingZ = false;
    private Rigidbody rb;

    public void Start()
    {
        // moveSequence = new Queue<MoveDirection>();
        // moveSequence.Enqueue(MoveDirection.Z);
        // moveSequence.Enqueue(MoveDirection.X);
        
        _speedX = GameManager.Instance.speedX;
        _speedY = GameManager.Instance.speedY;
        _speedZ = GameManager.Instance.speedZ;

        _directionX = new Vector2(_speedX, 0);
        _directionY = new Vector2(0, -_speedY);
        _directionZ = new Vector3(0, 0, _speedZ);

        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (_isMovingX)
        {
            MoveX();
        }
        else if(_isMovingZ)
        {
            MoveZ();
        }
    }
    
    // public MoveDirection GetNextDirection()
    // {
    //     MoveDirection direction = moveSequence.Dequeue();
    //     moveSequence.Enqueue(direction);
    //     return direction;
    // }
    

    public void StartMovingX()
    {
        Debug.Log("StartMovingX()");
        _isMovingX = true;
        _isMovingZ = false;
    }
    

    public void StartMovingZ()
    {
        Debug.Log("StartMovingZ()");
        _isMovingZ = true;
        _isMovingX = false;
    }

    private void MoveX()
    {
        // rb.AddForce(_directionX * (_speedX * Time.deltaTime));
        transform.Translate(_directionX * (_speedX * Time.deltaTime));
    }

    private void MoveY()
    {
        transform.Translate(_directionY * (_speedY * Time.deltaTime));
    }

    private void MoveZ()
    {
        // rb.AddForce(_directionZ * (_speedZ * Time.deltaTime));
        transform.Translate(_directionZ * (_speedZ * Time.deltaTime));
    }

    public void StopFullMoving()
    {
        _isMovingX = false;
        _isMovingZ = false;
    }
    
}
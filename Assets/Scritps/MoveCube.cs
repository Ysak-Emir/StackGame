using System;
using System.Collections.Generic;
using Scritps;
using UnityEngine;
using Random = System.Random;

public class MoveCube : MonoBehaviour
{
    
    
    public MoveCube Instance { get; private set; }
    [SerializeField] private CutBlock _cutBlock;
    [SerializeField] private CubeFactory _cubeFactory;
    
    
    private float _speedX;
    private float _speedY;
    private float _speedZ;
    private Vector2 _directionX;
    private Vector2 _directionY;
    private Vector3 _directionZ;
    
    
    public MoveDirection currentMoving;
    private Queue<MoveDirection> moveSequence;
    
    
    [SerializeField]private bool _isMovingX = false;
    public bool IsMovingX => _isMovingX;
    
    [SerializeField]public bool _isMovingZ = false; 
    public bool IsMovingZ => _isMovingZ;
    
    private bool _isMovingY = false;

    public float SpeedY
    {
        get { return _speedY; }
        private set { _speedY = Mathf.Clamp(value, 0, 3); }
    }

    public float SpeedX
    {
        get {return _speedX; }
        private set { _speedX = Mathf.Clamp(value, 0, 3); }
    }

    public float SpeedZ
    {
        get { return _speedZ; }
        private set { _speedZ = Mathf.Clamp(value, 0, 3); }
    }

    private void Awake()
    {
        // _cubeFactory = GetComponent<CubeFactory>();
        _cutBlock = GetComponent<CutBlock>();
    }

    public void InitMoveCube()
    {
        _cubeFactory = GetComponent<CubeFactory>();
        
        moveSequence = new Queue<MoveDirection>();
        moveSequence.Enqueue(MoveDirection.Z);
        moveSequence.Enqueue(MoveDirection.X);
        
        _speedX = GameManager.Instance.speedX;
        _speedY = GameManager.Instance.speedY;
        _speedZ = GameManager.Instance.speedZ;

        _directionX = new Vector2(_speedX, 0);
        _directionY = new Vector2(0, -_speedY);
        _directionZ = new Vector3(0, 0, _speedZ);
        
        if (_cubeFactory.randomSpawn == GameManager.Instance.spawnPointX.transform.position)
        {
            StartMovingX();
            currentMoving = MoveDirection.X;
        }
        else if (_cubeFactory.randomSpawn == GameManager.Instance.spawnPointZ.transform.position)
        {
            StartMovingZ();
            currentMoving = MoveDirection.Z;
            
        }
        else
        {
            Debug.Log("AZAZAZA");
        }
    }

    public void GetRandomSpawnCube()
    {
        
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
    
    public MoveDirection GetNextDirection()
    {
        MoveDirection direction = moveSequence.Dequeue();
        moveSequence.Enqueue(direction);
        return direction;
    }
    

    public void StartMovingX()
    {
        _isMovingX = true;
    }
    

    public void StartMovingZ()
    {
        _isMovingZ = true;
    }

    private void MoveX()
    {
        transform.Translate(_directionX * (_speedX * Time.deltaTime));
    }

    private void MoveY()
    {
        transform.Translate(_directionY * (_speedY * Time.deltaTime));
    }

    private void MoveZ()
    {
        transform.Translate(_directionZ * (_speedZ * Time.deltaTime));
    }

    public void StopFullMoving()
    {
        _isMovingX = false;
        _isMovingZ = false;
    }
    
}
using System;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    [SerializeField] [Range(0, 3)] private float speedX = 1f;
    [SerializeField] [Range(0, 3)] private float speedY = 1f;
    [SerializeField] [Range(0, 3)] private float speedZ = 1f;
    private Vector2 _directionX;
    private Vector2 _directionY;
    private Vector3 _directionZ;
    
    [SerializeField]private bool _isMovingX = false;
    public bool IsMovingX => _isMovingX;
    
    [SerializeField]public bool _isMovingZ = false; 
    public bool IsMovingZ => _isMovingZ;
    
    private bool _isMovingY = false;

    public float SpeedY
    {
        get { return speedY; }
        private set { speedY = Mathf.Clamp(value, 0, 3); }
    }

    public float SpeedX
    {
        get {return speedX; }
        private set { speedX = Mathf.Clamp(value, 0, 3); }
    }

    public float SpeedZ
    {
        get { return speedZ; }
        private set { speedZ = Mathf.Clamp(value, 0, 3); }
    }
    
    private void Awake()
    {
        _directionX = new Vector2(SpeedX, 0);
        _directionY = new Vector2(0, -SpeedY);
        _directionZ = new Vector3(0, 0, SpeedZ);
    }

    private void Update()
    {
        if (_isMovingX)
        {
            MoveX();
        }
        else if (_isMovingY)
        {
            MoveY();
        }
        else if(_isMovingZ)
        {
            MoveZ();
        }
    }

    public void StartMovingX()
    {
        _isMovingX = true;
    }

    public void StartMovingY()
    {
        _isMovingY = true;
    }

    public void StartMovingZ()
    {
        _isMovingZ = true;
    }

    private void MoveX()
    {
        transform.Translate(_directionX * (speedX * Time.deltaTime));
    }

    private void MoveY()
    {
        transform.Translate(_directionY * (speedY * Time.deltaTime));
    }

    private void MoveZ()
    {
        transform.Translate(_directionZ * (speedZ * Time.deltaTime));
    }
}
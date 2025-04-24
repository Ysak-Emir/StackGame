using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Vector3 _targetPosition;
    private float _moveSpeed = 1f;
    private bool _isMoving = false;

    void Start()
    {
        _camera = Camera.main;
        _targetPosition = _camera.transform.position + new Vector3(0, 0.1f, 0);
    }
    
    void Update()
    {
        if (_isMoving)
        {
            CameraUp();
        }
    }

    private void CameraUp()
    {
        _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
    }

    public void StartCamera()
    {
        _isMoving = true;
        _targetPosition += new Vector3(0, 0.1f, 0);
    }
    
}
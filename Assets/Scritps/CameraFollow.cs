using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera _camera;
    private bool _isMovingUp = false;
    private float _targetY;
    private float _moveSpeed = 1f; // скорость подъема

    void Start()
    {
        _camera = Camera.main; // или GetComponent<Camera>() если скрипт на камере
    }

    void Update()
    {
        if (_isMovingUp)
        {
            Vector3 currentPosition = _camera.transform.position;
            float newY = Mathf.MoveTowards(currentPosition.y, _targetY, _moveSpeed * Time.deltaTime);
            _camera.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);

            if (Mathf.Approximately(newY, _targetY))
            {
                _isMovingUp = false;
            }
        }
    }

    public void CameraUp()
    {
        _targetY = _camera.transform.position.y + 0.1f;
        _isMovingUp = true;
    }
}


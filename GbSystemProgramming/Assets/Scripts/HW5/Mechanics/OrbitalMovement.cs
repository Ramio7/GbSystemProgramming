using Unity.Netcode;
using UnityEngine;

public class OrbitalMovement : NetworkBehaviour
{
    [SerializeField] private Transform _aroundPoint;
    [SerializeField] private float _smoothTime = .3f;
    [SerializeField] private float _circleInSecond = 1f;
    [SerializeField] private float _offsetSin = 1;
    [SerializeField] private float _offsetCos = 1;
    [SerializeField] private float _rotationSpeed;
    private float _dist;
    private float _currentAng;
    private float _currentRotationAngle;
    private const float _circleRadians = Mathf.PI * 2;

    private void Start()
    {
        
        _dist = (transform.position - _aroundPoint.position).magnitude;
        EntryPoint.OnFixedUpdate += Move;
    }

    private void Move()
    {
        if (!IsServer) return;

        var p = _aroundPoint.position;
        p.x += Mathf.Sin(_currentAng) * _dist * _offsetSin;
        p.z += Mathf.Cos(_currentAng) * _dist * _offsetCos;
        transform.position = p;
        _currentRotationAngle += Time.deltaTime * _rotationSpeed;
        _currentRotationAngle = Mathf.Clamp(_currentRotationAngle, 0, 361);
        if (_currentRotationAngle >= 360)
        {
            _currentRotationAngle = 0;
        }
        transform.rotation = Quaternion.AngleAxis(_currentRotationAngle,
        transform.up);
        _currentAng += _circleRadians * _circleInSecond * Time.deltaTime;
    }
}

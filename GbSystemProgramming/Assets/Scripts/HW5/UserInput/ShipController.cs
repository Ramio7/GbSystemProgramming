using Unity.Netcode;
using UnityEngine;

public class ShipController : NetworkBehaviour
{

    [SerializeField] private Transform _cameraAttach;
    private CameraInput _cameraInput;
    private PlayerLabel _playerLabel;
    private float _shipSpeed;
    private Rigidbody _rb;
    private string _playerName;
    private SpaceShipSettings _spaceShipSettings;
    private bool _isFaster;
    private float _currentFov;

    public string PlayerName
    {
        get => _playerName;
        set => _playerName = value;
    }

    private void Start()
    {
        OnStartAuthority();
        EntryPoint.OnFixedUpdate += HasAuthorityMovement;
        EntryPoint.OnLateUpdate += _cameraInput.CameraMovement;
    }

    private void OnGUI()
    {
        if (_cameraInput == null)
        {
            return;
        }
        _cameraInput.ShowPlayerLabels(_playerLabel);
    }

    private void OnStartAuthority()
    {
        if (!TryGetComponent(out _rb)) return;

        _spaceShipSettings = HW5EntryPoint.ShipSettings;
        _currentFov = _spaceShipSettings.NormalFov;

        gameObject.name = _playerName;
        _cameraInput = FindObjectOfType<CameraInput>();
        _cameraInput.Initiate(_cameraAttach == null ? transform : _cameraAttach);
        _playerLabel = GetComponentInChildren<PlayerLabel>();
    }

    private void HasAuthorityMovement()
    {
        if (_spaceShipSettings == null) return;

        if (Input.GetKey(KeyCode.LeftShift).CompareTo(_isFaster) == 0)
        {
            var faster = _isFaster ? _spaceShipSettings.Faster : 1.0f;
            _shipSpeed = Mathf.Lerp(_shipSpeed, _spaceShipSettings.ShipSpeed * faster, _spaceShipSettings.Acceleration);
            _currentFov = _isFaster ? _spaceShipSettings.FasterFov : _spaceShipSettings.NormalFov;
        }
        
        _cameraInput.SetFov(_currentFov, _spaceShipSettings.ChangeFovSpeed);
        _rb.velocity = _cameraInput.transform.TransformDirection(Vector3.forward) * _shipSpeed;
        if (!Input.GetKey(KeyCode.C))
        {
            var targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(_cameraInput.LookAngle, -transform.right) *
                _cameraInput.transform.TransformDirection(Vector3.forward) * _shipSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation,
            targetRotation, Time.deltaTime * _spaceShipSettings.ShipSpeed);
        }
    }
}

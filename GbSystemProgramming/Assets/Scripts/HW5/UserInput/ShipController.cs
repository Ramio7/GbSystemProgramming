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
    public string PlayerName
    {
        get => _playerName;
        set => _playerName = value;
    }

    private void Start()
    {
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

    public void OnStartAuthority()
    {
        if (!TryGetComponent(out _rb))
        {
            return;
        }
        gameObject.name = _playerName;
        _cameraInput = FindObjectOfType<CameraInput>();
        _cameraInput.Initiate(_cameraAttach == null ? transform : _cameraAttach);
        _playerLabel = GetComponentInChildren<PlayerLabel>();
    }

    protected  void HasAuthorityMovement()
    {
        var spaceShipSettings = HW5EntryPoint.SpaceShipSettings;
        if (spaceShipSettings == null)
        {
            return;
        }
        var isFaster = Input.GetKey(KeyCode.LeftShift);
        var speed = spaceShipSettings.ShipSpeed;
        var faster = isFaster ? spaceShipSettings.Faster : 1.0f;
        _shipSpeed = Mathf.Lerp(_shipSpeed, speed * faster, spaceShipSettings.Acceleration);
        var currentFov = isFaster ? spaceShipSettings.FasterFov : spaceShipSettings.NormalFov;
        _cameraInput.SetFov(currentFov, spaceShipSettings.ChangeFovSpeed);
        var velocity = _cameraInput.transform.TransformDirection(Vector3.forward) * _shipSpeed;
        _rb.velocity = velocity * Time.deltaTime;
        if (!Input.GetKey(KeyCode.C))
        {
            var targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(_cameraInput.LookAngle, -transform.right) * velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation,
            targetRotation, Time.deltaTime * speed);
        }
    }
}

using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class ShipController : NetworkBehaviour
{
    [SerializeField] private Camera _cameraAttach;
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

    private void OnEnable()
    {
        OnStartAuthority();
        SubscribeUpdates();
    }

    private void OnDisable()
    {
        UnsubscribeUpdates();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeUpdates();
    }
    private void OnGUI()
    {
        if (_cameraInput == null)
        {
            return;
        }
        _cameraInput.ShowPlayerLabels(_playerLabel);
    }

    private void OnCollisionEnter(Collision collision)
    {
        RespawnPlayerServerRpc();
    }

    [ServerRpc]
    private void RespawnPlayerServerRpc()
    {
        gameObject.SetActive(false);
        WaitInSeconds(5);
        var spawnPoint = EntryPoint.SpawnPointsManager.GetSpawnPoint();
        gameObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        gameObject.SetActive(true);
    }

    private async void WaitInSeconds(float seconds)
    {
        await Task.Delay((int)(seconds * 1000));
    }

    private void SubscribeUpdates()
    {
        EntryPoint.OnFixedUpdate += HasAuthorityMovement;
        EntryPoint.OnLateUpdate += _cameraInput.CameraMovement;
    }

    private void UnsubscribeUpdates()
    {
        EntryPoint.OnFixedUpdate -= HasAuthorityMovement;
        EntryPoint.OnLateUpdate -= _cameraInput.CameraMovement;
    }

    private void OnStartAuthority()
    {
        if (!TryGetComponent(out _rb)) return;

        _spaceShipSettings = HW5EntryPoint.ShipSettings;
        _currentFov = _spaceShipSettings.NormalFov;

        _cameraInput = GetComponent<CameraInput>();
        _cameraInput.Initiate(_cameraAttach == null ? Camera.main : _cameraAttach);
        _playerLabel = GetComponent<PlayerLabel>();
    }

    private void HasAuthorityMovement()
    {
        if (_spaceShipSettings == null) return;

        float faster = 0f;

        if (Input.GetKey(KeyCode.LeftShift) == _isFaster)
        {
            faster = _isFaster ? _spaceShipSettings.Faster : 1.0f;
        }

        _shipSpeed = Mathf.Lerp(_shipSpeed, _spaceShipSettings.ShipSpeed * faster, _spaceShipSettings.Acceleration);
        _currentFov = _isFaster ? _spaceShipSettings.FasterFov : _spaceShipSettings.NormalFov;

        _cameraInput.SetFov(_currentFov, _spaceShipSettings.ChangeFovSpeed);
        _rb.AddForce(_shipSpeed * Time.deltaTime * _cameraInput.transform.TransformDirection(Vector3.forward), ForceMode.VelocityChange);

        if (!Input.GetKey(KeyCode.C))
        {
            var targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(_cameraInput.LookAngle, -transform.right) *
                _cameraInput.transform.TransformDirection(Vector3.forward) * _shipSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation,
            targetRotation, Time.deltaTime * _spaceShipSettings.ShipSpeed);
        }

        Debug.Log($"{_rb.velocity} {transform.position}");
    }
}

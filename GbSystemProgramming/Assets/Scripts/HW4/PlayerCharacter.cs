using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MouseLook))]
public class PlayerCharacter : Character
{
    [Range(0, 100)][SerializeField] private int health = 100;
    [Range(0.5f, 10.0f)][SerializeField] private float movingSpeed = 8.0f;
    [SerializeField] private float acceleration = 3.0f;
    private const float gravity = -9.8f;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private MouseLook _mouseLook;
    protected override FireAction FireAction { get; set; }

    private void Start()
    {
        Initiate();
    }

    protected override void Initiate()
    {
        base.Initiate();
        FireAction = gameObject.AddComponent<RayShooter>();
        FireAction.Reloading();
        _characterController = _characterController != null ? _characterController : gameObject.AddComponent<CharacterController>();
        _mouseLook = _mouseLook != null ? _mouseLook : gameObject.AddComponent<MouseLook>();
    }

    public override void Movement()
    {
        if (_mouseLook != null && _mouseLook.PlayerCamera != null)
        {
            _mouseLook.PlayerCamera.enabled = IsOwner;
        }

        if (IsOwner)
        {
            var moveX = Input.GetAxis("Horizontal") * movingSpeed;
            var moveZ = Input.GetAxis("Vertical") * movingSpeed;
            var movement = new Vector3(moveX, 0, moveZ);
            movement = Vector3.ClampMagnitude(movement, movingSpeed);
            movement *= Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movement *= acceleration;
            }
            movement.y = gravity;
            movement = transform.TransformDirection(movement);
            _characterController.Move(movement);
            _mouseLook.Rotation();
        }
    }

    private void OnGUI()
    {
        if (Camera.main == null)
        {
            return;
        }
        var info = $"Health: {health}\nClip: {FireAction.Bullets.Count}";
        var size = 12;
        var bulletCountSize = 50;
        var posX = Camera.main.pixelWidth / 2 - size / 4;
        var posY = Camera.main.pixelHeight / 2 - size / 2;
        var posXBul = Camera.main.pixelWidth - bulletCountSize * 2;
        var posYBul = Camera.main.pixelHeight - bulletCountSize;
        GUI.Label(new Rect(posX, posY, size, size), "+");
        GUI.Label(new Rect(posXBul, posYBul, bulletCountSize * 2,
        bulletCountSize * 2), info);
    }
}

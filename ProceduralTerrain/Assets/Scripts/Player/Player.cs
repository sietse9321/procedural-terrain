using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    private IPlayerInput _input;
    private IMovement _movement;
    private Jump _jump;
    private Rigidbody _rb;

    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;

    private Camera _mainCamera;

    private void Awake()
    {
        _input = GetComponent<IPlayerInput>();
        _movement = GetComponent<Movement>();
        _jump = GetComponent<Jump>();
        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_mainCamera == null) return;

        Vector3 cameraForward = _mainCamera.transform.forward;
        Vector3 cameraRight = _mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector2 movementInput = _input.GetMovementInput();

        Vector3 inputDir = cameraForward * movementInput.y + cameraRight * movementInput.x;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        _movement.Move(inputDir);

        if (_jump)
        {
            _jump.TryJump();
        }
    }
}
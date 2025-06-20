using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    private IPlayerInput _input;
    private IAttackCombo _attackCombo;
    private IMovement _movement;
    private IDashable _dash;
    private IHealth _health;

    private CamTargetLock _camTargetLock;

    //private Jump _jump;
    private Rigidbody _rb;


    private Camera _mainCamera;

    private void Awake()
    {
        _input = GetComponent<IPlayerInput>();
        _dash = GetComponent<IDashable>();
        _movement = GetComponent<Movement>();
        _health = GetComponent<IHealth>();
        _camTargetLock = GetComponent<CamTargetLock>();
        //_jump = GetComponent<Jump>();
        _rb = GetComponent<Rigidbody>();
        _attackCombo = GetComponent<IAttackCombo>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _mainCamera = Camera.main;
        _movement.MoveSpeed = moveSpeed;
    }

    private void Update()
    {
        MovementByCamera();
        InputUpdate();
    }

    private void InputUpdate()
    {
        if (_input.GetTargerLockInput())
        {
            _camTargetLock.TargetLock();
        }

       
        if (_input.GetDashInput() && !_camTargetLock.IsTargetLocked)
        {
            _dash?.DashDirection(playerObj.forward);
        }

        // Target switching with input abstraction
        int switchInput = _input.GetTargetSwitchInput();
        if (switchInput != 0)
        {
            _camTargetLock?.SwitchTarget(switchInput);
        }

        if (_input.GetAttackInput())
        {
            _attackCombo?.Attack();
            Vector3 toTarget = (_camTargetLock.CurrentTarget.transform.position - playerObj.position).normalized;
            _dash?.DashDirection(toTarget);
        }
    }

    private void MovementByCamera()
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
    }
}
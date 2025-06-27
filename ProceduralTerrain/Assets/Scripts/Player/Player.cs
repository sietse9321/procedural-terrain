using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    private InputSwitcher _inputSwitcher;
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
        _inputSwitcher = GetComponent<InputSwitcher>();
        _input = _inputSwitcher.ActiveInput;
        _dash = GetComponent<IDashable>();
        _movement = GetComponent<Movement>();
        _health = GetComponent<IHealth>();
        _camTargetLock = GetComponent<CamTargetLock>();
        _rb = GetComponent<Rigidbody>();
        _attackCombo = GetComponent<IAttackCombo>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _mainCamera = Camera.main;
        _movement.MoveSpeed = moveSpeed;
    }

    private void Update()
    {
        InputUpdate();
        MovementByCamera();
    }

    private void InputUpdate()
    {
        _input = _inputSwitcher.ActiveInput;

        if (_input.GetTargerLockInput())
        {
            _camTargetLock.TargetLock();
        }

        if (_input.GetDashInput() && !_camTargetLock.IsTargetLocked)
        {
            _dash?.DashDirection(playerObj.forward);
        }

        int switchInput = _input.GetTargetSwitchInput();
        if (switchInput != 0 && _camTargetLock.IsTargetLocked)
        {
            _camTargetLock?.SwitchTarget(switchInput);
        }

        if (_input.GetAttackInput())
        {
            _attackCombo?.Attack();
            if (_camTargetLock.IsTargetLocked)
            {
                Vector3 toTarget = (_camTargetLock.CurrentTarget.TargetTransform.position - playerObj.position).normalized;
                _dash?.DashDirection(toTarget);
            }
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
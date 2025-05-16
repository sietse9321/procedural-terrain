using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    private IPlayerInput _input;
    private IMovement _movement;

    private void Awake()
    {
        _input = GetComponent<IPlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        Vector2 movementInput = _input.GetMovementInput();
        _movement.Move(movementInput);
    }
}
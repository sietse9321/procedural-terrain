using UnityEngine;

public class Movement : MonoBehaviour, IMovement
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private float groundCheckDistance = 1f; 
    
    private Rigidbody rb;
    private int jumpsRemaining;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpsRemaining = maxJumps;
    }
    public void Move(Vector2 direction)
    {
        Vector3 movement = new Vector3(direction.x, 0f, direction.y);

        movement = Camera.main.transform.TransformDirection(movement);

        movement = movement.normalized * moveSpeed * Time.deltaTime;

        transform.Translate(movement, Space.World);
    }
}
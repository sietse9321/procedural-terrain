using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private float groundCheckDistance = 1f;

    private Rigidbody rb;
    private int jumpsRemaining;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ResetJumpCount();
    }

    private void Update()
    {
        CheckGround();
    }

    public void TryJump()
    {
        if (isGrounded || jumpsRemaining > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpsRemaining--;
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        if (isGrounded)
        {
            ResetJumpCount();
        }
    }

    private void ResetJumpCount()
    {
        jumpsRemaining = maxJumps;
    }
}
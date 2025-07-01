using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private float groundCheckDistance = 1f;

    private Rigidbody _rb;
    private int _jumpsRemaining;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        ResetJumpCount();
    }

    private void Update()
    {
        CheckGround();
    }

    public void TryJump()
    {
        if (_isGrounded || _jumpsRemaining > 0)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpsRemaining--;
        }
    }

    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        if (_isGrounded)
        {
            ResetJumpCount();
        }
    }

    private void ResetJumpCount()
    {
        _jumpsRemaining = maxJumps;
    }
}
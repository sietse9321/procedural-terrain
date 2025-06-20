using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Dash : MonoBehaviour, IDashable
{
    [SerializeField] private float dashDistance = 2f;
    [SerializeField] private float dashDuration = 0.05f;
    [SerializeField] private float cooldown = 1f;

    public bool IsDashing { get; private set; }

    private Rigidbody _rb;
    private float _lastDashTime = -Mathf.Infinity;
    private Vector3 _dashDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void DashDirection(Vector3 direction)
    {
        if (IsDashing || Time.time < _lastDashTime + cooldown) return;
        if (direction == Vector3.zero) return;

        _dashDirection = direction.normalized;
        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        IsDashing = true;
        float startTime = Time.time;

        Vector3 dashVelocity = _dashDirection * (dashDistance / dashDuration);
        Vector3 originalVelocity = _rb.velocity;

        while (Time.time < startTime + dashDuration)
        {
            _rb.velocity = dashVelocity;
            yield return null;
        }

        // Restore control
        _rb.velocity = originalVelocity;
        _lastDashTime = Time.time;
        IsDashing = false;
    }
}
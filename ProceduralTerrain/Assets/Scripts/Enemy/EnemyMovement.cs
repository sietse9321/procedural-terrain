using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private IDashable _dashable;
    private float _nextDashTime;

    public float dashCooldown = 5f;

    [Range(0f, 1f)] 
    public float dashChance = 0.1f;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _dashable = GetComponent<IDashable>();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.player) return;

        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);

        if (distanceToPlayer <= 10f)
        {
            _agent.destination = GameManager.Instance.player.transform.position;

            AttemptDash();
        }
    }

    /// <summary>
    /// Tries to dash in a random direction
    /// </summary>
    private void AttemptDash()
    {
        if (_dashable == null) return;
        if (_dashable.IsDashing) return;
        if (Time.time < _nextDashTime) return;
        if (Random.value > dashChance) return;

        Vector2 randomCircle = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(randomCircle.x, 0f, randomCircle.y);

        _dashable.DashDirection(direction);

        _nextDashTime = Time.time + dashCooldown;
    }
}
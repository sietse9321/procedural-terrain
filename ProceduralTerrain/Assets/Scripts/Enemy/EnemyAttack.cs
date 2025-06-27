using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageCooldown = 1f;
    private float lastDamageTime = -Mathf.Infinity;

    private Player player;

    private void Start()
    {
        // Assumes GameManager exposes a public player reference or property
        player = GameManager.Instance.player;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time - lastDamageTime >= damageCooldown)
        {
            if (collision.gameObject.TryGetComponent(out IHealth health))
            {
                lastDamageTime = Time.time;
                health.TakeDamage(damageAmount);
            }
        }
    }
}
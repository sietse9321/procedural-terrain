using Interfaces;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageCooldown = 1f;
    private float _lastDamageTime = -Mathf.Infinity;

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time - _lastDamageTime >= damageCooldown)
        {
            if (collision.gameObject.TryGetComponent(out IHealth health) && !collision.gameObject.TryGetComponent<IEnemy>(out _))
            {
                _lastDamageTime = Time.time;
                health.TakeDamage(damageAmount);
            }
        }
    }
}
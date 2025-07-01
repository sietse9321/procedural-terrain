using Interfaces;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private SwordAttackCombo _attackCombo;
    
    /// <summary>
    /// Gets all hittables in the collider and invokes their OnHit event
    /// If the collider has a IHealth interface, it takes damage
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        IHittable[] hittables = other.GetComponents<IHittable>();
        foreach (var hittable in hittables)
        {
            hittable.OnHit();
        }

        if (other.TryGetComponent(out IHealth health))
        {
            health.TakeDamage(10);
        }
    }
    
    public void OnAttackAnimationEnd()
    {
        _attackCombo?.OnAttackAnimationEnd();
    }

    private void Awake()
    {
        if (_attackCombo == null)
        {
            _attackCombo = GetComponentInParent<SwordAttackCombo>();
        }
    }
}
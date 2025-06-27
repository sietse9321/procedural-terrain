using UnityEngine;

[RequireComponent(typeof(IHealth))]
public class RangedEnemy : MonoBehaviour, ITargetable
{
    private IHealth _health;
    public Transform TargetTransform => transform;

    private void Awake()
    {
        _health = GetComponent<IHealth>();
        
        if (_health is Health healthComponent)
        {
            healthComponent.OnTakeDamage += DamageEffect;
            healthComponent.OnDeath += HandleDeath;
        }
        if (_health is ArmouredHealth aHealthComponent)
        {
            aHealthComponent.OnTakeDamage += DamageEffect;
            aHealthComponent.OnDeath += HandleDeath;
        }

        _health.MaxHealth = 75;
    }

    private void DamageEffect(int damageAmount)
    {
        Debug.Log($"Enemy {gameObject.name} took {damageAmount} damage");

        //add damage effect here
        //damage number effect
    }

    private void HandleDeath()
    {
        Debug.Log($"{gameObject.name} has died");
        if (_health is Health healthComponent)
        {
            healthComponent.OnTakeDamage -= DamageEffect;
            healthComponent.OnDeath -= HandleDeath;
        }
        if (_health is ArmouredHealth aHealthComponent)
        {
            aHealthComponent.OnTakeDamage -= DamageEffect;
            aHealthComponent.OnDeath -= HandleDeath;
        }
        Destroy(gameObject);
    }
}
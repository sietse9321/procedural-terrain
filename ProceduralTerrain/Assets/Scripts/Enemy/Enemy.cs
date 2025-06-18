using System;
using UnityEngine;

[RequireComponent(typeof(IHealth))]
public class Enemy : MonoBehaviour
{
    private IHealth _health;

    private void Awake()
    {
        _health = GetComponent<IHealth>();
        
        if (_health is Health healthComponent)
        {
            healthComponent.OnTakeDamage += DamageEffect;
            healthComponent.OnDeath += HandleDeath;
        }
    }

    private void DamageEffect(int damageAmount)
    {
        Debug.Log($"Enemy {gameObject.name} took {damageAmount} damage");

        //add damage effect here
    }

    private void HandleDeath()
    {
        Debug.Log($"{gameObject.name} has died");
        if (_health is Health healthComponent)
        {
            healthComponent.OnTakeDamage -= DamageEffect;
            healthComponent.OnDeath -= HandleDeath;
        }
        Destroy(gameObject);
    }
}
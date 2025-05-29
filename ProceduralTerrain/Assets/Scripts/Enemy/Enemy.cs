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
            healthComponent.OnTakeDamage += HandleDamageTaken;
        }
    }

    private void HandleDamageTaken(int damageAmount)
    {
        Debug.Log($"Enemy {gameObject.name} took {damageAmount} damage!");

        //add effect/shader effect (for damage)
    }

    private void OnDestroy()
    {
        if (_health is Health healthComponent)
        {
            healthComponent.OnTakeDamage -= HandleDamageTaken;
        }
    }
}
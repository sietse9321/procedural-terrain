using UnityEngine;

public interface IHealth
{
    // Current health of the entity
    int CurrentHealth { get; }

    // Maximum health of the entity
    int MaxHealth { get; set; }

    // Method to apply damage to the entity
    void TakeDamage(int damageAmount);

    // Method to heal the entity
    void Heal(int healAmount);

    // Check if the entity is still alive
    bool IsAlive();

}
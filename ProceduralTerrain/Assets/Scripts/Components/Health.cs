using UnityEngine;
using System;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public event Action<int> OnTakeDamage;
    public event Action OnDeath;

    public int CurrentHealth => currentHealth;

    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = Mathf.Max(0, value);
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int pDamage)
    {
        if (pDamage <= 0 || currentHealth <= 0) return;

        currentHealth -= pDamage;
        OnTakeDamage?.Invoke(pDamage);
        Debug.Log($"{gameObject.name} took {pDamage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int pHeal)
    {
        if (pHeal <= 0 || currentHealth == maxHealth) return;

        currentHealth += pHeal;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} healed for {pHeal}. Current health: {currentHealth}");
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}
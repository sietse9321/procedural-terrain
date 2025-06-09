using UnityEngine;
using System;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    // Event triggered when the entity takes damage
    public event Action<int> OnTakeDamage;
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth { get => maxHealth; set => maxHealth = Mathf.Max(0, value); }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int pDamage)
    {
        if (pDamage <= 0 || currentHealth <= 0) return;

        currentHealth -= pDamage;

        Debug.Log($"{gameObject.name} took {pDamage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
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

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        // Logic for death...
    }
}
/*using UnityEngine;
   using System;
   
   public class Health : MonoBehaviour, IHealth
   {
       [SerializeField] private int maxHealth = 100;
       private int currentHealth;
   
       // Event triggered when the entity takes damage
       public event Action<int> OnTakeDamage;
   
       // Event triggered when the entity dies
       public event Action OnDeath;
   
       public int CurrentHealth => currentHealth;
       public int MaxHealth { get => maxHealth; set => maxHealth = Mathf.Max(0, value); }
   
       private void Awake()
       {
           currentHealth = maxHealth;
       }
   
       public void TakeDamage(int damageAmount)
       {
           if (damageAmount <= 0 || currentHealth <= 0) return;
   
           currentHealth -= damageAmount;
   
           Debug.Log($"{gameObject.name} took {damageAmount} damage. Remaining health: {currentHealth}");
   
           OnTakeDamage?.Invoke(damageAmount);
   
           if (currentHealth <= 0)
           {
               Die();
           }
       }
   
       public void Heal(int healAmount)
       {
           if (healAmount <= 0 || currentHealth == maxHealth) return;
   
           currentHealth += healAmount;
           currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
   
           Debug.Log($"{gameObject.name} healed for {healAmount}. Current health: {currentHealth}");
       }
   
       public bool IsAlive()
       {
           return currentHealth > 0;
       }
   
       private void Die()
       {
           Debug.Log($"{gameObject.name} has died!");
           OnDeath?.Invoke();
           // Logic for death...
       }
   }
   */
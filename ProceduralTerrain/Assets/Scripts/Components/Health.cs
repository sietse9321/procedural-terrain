using UnityEngine;
using System;
using Interfaces;

namespace Components
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField] private int maxHealth;
        private int _currentHealth;

        public event Action<int> OnTakeDamage;
        public event Action OnDeath;

        public int CurrentHealth => _currentHealth;

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = Mathf.Max(0, value);
        }

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        /// <summary>
        /// Reduces the health by the incoming damage
        /// invokes OnTakeDamage event
        /// invokes OnDeath event if health is 0 or less
        /// </summary>
        /// <param name="pDamage"></param>
        public void TakeDamage(int pDamage)
        {
            if (pDamage <= 0 || _currentHealth <= 0) return;

            _currentHealth -= pDamage;
            OnTakeDamage?.Invoke(pDamage);
            Debug.Log($"{gameObject.name} took {pDamage} damage. Remaining health: {_currentHealth}");

            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        /// <summary>
        /// Increase the health by the incoming heal
        /// </summary>
        /// <param name="pHeal"></param>
        public void Heal(int pHeal)
        {
            if (pHeal <= 0 || _currentHealth == maxHealth) return;

            _currentHealth += pHeal;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

            Debug.Log($"{gameObject.name} healed for {pHeal}. Current health: {_currentHealth}");
        }
        
        public bool IsAlive()
        {
            return _currentHealth > 0;
        }
    }
}
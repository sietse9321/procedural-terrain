using System;
using Interfaces;
using UnityEngine;

namespace Components
{
    public class ArmouredHealth : MonoBehaviour, IHealth
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
        
        public void TakeDamage(int pDamage)
        {
            if (pDamage <= 0 || _currentHealth <= 0) return;

            // Take only 80% of the incoming damage
            int reducedDamage = Mathf.CeilToInt(pDamage * 0.8f);

            _currentHealth -= reducedDamage;
            OnTakeDamage?.Invoke(reducedDamage);
            Debug.Log($"{gameObject.name} took {reducedDamage} damage. Remaining health: {_currentHealth}");

            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }


        public void Heal(int pHeal)
        {
            if (pHeal <= 0 || _currentHealth == MaxHealth) return;

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
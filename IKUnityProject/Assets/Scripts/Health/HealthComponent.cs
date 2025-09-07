using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Health
{
    public class HealthComponent : MonoBehaviour
    {
        public Action<int> OnChangeHealth;
        
        public int MaxHealth => _maxHealth;
        [SerializeField] private int _maxHealth = 100;
        public int StartingHealth => _startingHealth;
        [SerializeField] private int _startingHealth = 100;
        public int CurrentHealth => _currentHealth;
        [ReadOnly] [SerializeField] private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _startingHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            OnChangeHealth?.Invoke(_currentHealth);
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        }
    }
}
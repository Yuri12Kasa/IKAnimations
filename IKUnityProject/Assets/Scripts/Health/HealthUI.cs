using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class HealthUI : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private Image _healthBar;

        private int _maxHealth;

        public void SetHealthComponent(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
            _healthComponent.OnChangeHealth += ResizeHealthBar;
            _maxHealth = _healthComponent.MaxHealth;
            ResizeHealthBar(_healthComponent.CurrentHealth);
        }

        private void ResizeHealthBar(int currentHealth)
        {
            _healthBar.fillAmount = currentHealth / (float)_maxHealth;
        }
    }
}
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class HealthUI : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private Image _healthBar;
        [SerializeField] private float _resizingDuration = 0.2f;

        private int _maxHealth;
        private int _currentHealth;
        private Coroutine _resizingCoroutine;

        public void SetHealthComponent(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
            _healthComponent.OnChangeHealth += StartResizeAnimation;
            _maxHealth = _healthComponent.MaxHealth;
            ResizeHealthBar(_healthComponent.CurrentHealth);
        }

        private void ResizeHealthBar(int newCurrentHealth)
        {
            _healthBar.fillAmount = newCurrentHealth / (float)_maxHealth;
            _currentHealth = newCurrentHealth;
        }

        private void StartResizeAnimation(int newCurrentHealth)
        {
            if(_resizingCoroutine != null)
                StopCoroutine(_resizingCoroutine);
            
            _resizingCoroutine = StartCoroutine(ResizeHealthBarCoroutine(newCurrentHealth));
        }

        private IEnumerator ResizeHealthBarCoroutine(int newCurrentHealth)
        {
            var timer = 0f;
            var startValue = _currentHealth;
            var endValue = (float)newCurrentHealth;
            while (timer <= _resizingDuration)
            {
                timer += Time.deltaTime;
                var t = timer / _resizingDuration;
                _healthBar.fillAmount = Mathf.Lerp(startValue, endValue, t) / _maxHealth;
                _currentHealth = newCurrentHealth;
                yield return null;
            }

            _resizingCoroutine = null;
        }
    }
}
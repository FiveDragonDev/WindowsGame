using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public static PlayerHealth Singleton { get; private set; }

    public bool Shielded => Time.time < _nextDamageTime;
    public float HealthPercent => CurrentHealth / MaxHealth;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _health;

    public UnityEvent<float> OnHeal => _onHeal;
    public UnityEvent<float> OnDamage => _ondamage;
    public UnityEvent OnDie => _onDie;

    [SerializeField] private Image _healthSlider;
    [SerializeField, Min(0)] private float _maxHealth = 1;

    private float _health;

    private readonly UnityEvent<float> _onHeal = new();
    private readonly UnityEvent<float> _ondamage = new();
    private readonly UnityEvent _onDie = new();

    private float _nextDamageTime;

    private void Awake()
    {
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        _health = MaxHealth;
        _healthSlider.fillAmount = HealthPercent;
    }

    public void Heal(float amount)
    {
        OnHeal?.Invoke(amount);
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

        _health += amount;
        if (_health > _maxHealth) _health = _maxHealth;
        _healthSlider.fillAmount = HealthPercent;
    }
    public void Damage(float amount)
    {
        OnDamage?.Invoke(amount);
        if (Shielded) return;
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

        _health -= amount;
        if (_health <= 0) Die();
        _healthSlider.fillAmount = HealthPercent;
        _nextDamageTime = Time.time + 0.1f;
    }
    public void Die()
    {
        OnDie?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

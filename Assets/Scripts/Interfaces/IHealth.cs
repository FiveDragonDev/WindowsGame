using UnityEngine.Events;

public interface IHealth
{
    public bool CanHeal => CurrentHealth < MaxHealth;
    public float HealthPercent => CurrentHealth / MaxHealth;

    public float MaxHealth { get; }
    public float CurrentHealth { get; }

    public UnityEvent<float> OnHeal { get; }
    public UnityEvent<float> OnDamage { get; }
    public UnityEvent OnDie { get; }

    public void Heal(float amount);
    public void Damage(float amount);

    void Die();
}

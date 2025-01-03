using UnityEngine.Events;

public interface IHealth
{
    public bool CanHeal => Health < MaxHealth;
    public float HealthPercent => Health / MaxHealth;

    public float MaxHealth { get; }
    public float Health { get; }

    public UnityEvent<float> OnHeal { get; }
    public UnityEvent<float> OnDamage { get; }
    public UnityEvent OnDie { get; }

    public void Heal(float amount);
    public void Damage(float amount);

    void Die();
}

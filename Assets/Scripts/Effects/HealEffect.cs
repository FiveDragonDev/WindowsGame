using UnityEngine;
using UnityEngine.Events;

public sealed class HealEffect : IEffect
{
    public UnityEvent OnApply => _onApply;

    public float HealAmount { get; }

    private readonly UnityEvent _onApply = new();

    public HealEffect(float healAmount) => HealAmount = healAmount;

    public void Apply()
    {
        var health = PlayerHealth.Singleton;
        if (health.Health >= health.MaxHealth) return;
        health.Heal(HealAmount);
        OnApply?.Invoke();
    }
}

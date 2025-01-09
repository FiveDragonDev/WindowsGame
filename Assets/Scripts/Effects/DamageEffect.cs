using UnityEngine;
using UnityEngine.Events;

public class DamageEffect : IEffect
{
    public UnityEvent OnApply => _onApply;

    public float DamageAmount { get; }

    private readonly UnityEvent _onApply = new();

    public DamageEffect(float damageAmount) => DamageAmount = damageAmount;

    public void Apply(IEffectable other) => OnApplyToBody(other);

    protected virtual void OnApplyToBody(IEffectable other)
    {
        if (other == null || other.Health == null) return;
        other.Health.Damage(DamageAmount);
        OnApply?.Invoke();
    }
}

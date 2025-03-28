using UnityEngine;

public sealed class DamageZone : EffectSource
{
    public override bool CanPickup => false;

    [SerializeField, Min(0)] private float _damageAmount = 1;
    [SerializeField, Min(0)] private float _damageInterval = 1;

    private float _nextDamageTime;

    private void Start()
    {
        _effect = new DamageEffect(_damageAmount);
        _nextDamageTime = Time.time + _damageInterval;
    }

    private void Update()
    {
        if (Entered.Count <= 0 || Time.time < _nextDamageTime) return;
        foreach (var entered in Entered)
            if (entered != null) Damage(entered);
    }

    private void Damage(IEffectable effectable)
    {
        _effect.Apply(effectable);
        _nextDamageTime = Time.time + _damageInterval;
    }
}

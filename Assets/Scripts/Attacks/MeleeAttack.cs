using UnityEngine;
using UnityEngine.Events;

public class MeleeAttack : IAttack
{
    public bool CanAttack => Time.time > _nextDamageTime;

    public UnityEvent OnAttack => _onAttack;

    public float Damage { get => _damage; set => _damage = value; }
    public float AttackRate { get => _attackRate; set => _attackRate = value; }

    private readonly UnityEvent _onAttack = new();
    private float _damage;
    private float _attackRate;

    private float _nextDamageTime;

    public MeleeAttack(float damage, float attackRate)
    {
        _damage = damage;
        _attackRate = attackRate;
    }

    public void Attack()
    {
        if (!CanAttack) return;
        PlayerHealth.Singleton.Damage(Damage);
        OnAttack?.Invoke();
        _nextDamageTime = Time.time + 1 / _attackRate;
    }
}

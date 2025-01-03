using UnityEngine;
using UnityEngine.Events;

public interface IAttack
{
    public bool CanAttack { get; }
    public UnityEvent OnAttack { get; }

    public float Damage { get; set; }
    public float AttackRate { get; }

    public void Attack();
}

using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IHealth
{
    public float MaxHealth => _maxHealth;
    public float Health => _health;

    public UnityEvent<float> OnHeal => _onHeal;
    public UnityEvent<float> OnDamage => _ondamage;
    public UnityEvent OnDie => _onDie;

    [SerializeField, Min(0)] private float _speed = 1;
    [SerializeField, Min(0)] private float _maxHealth = 1;
    [SerializeField, Min(0)] private float _damage = 1;
    [SerializeField, Min(0)] private float _attackRate = 1;

    private float _health;
    private readonly UnityEvent<float> _onHeal = new();
    private readonly UnityEvent<float> _ondamage = new();
    private readonly UnityEvent _onDie = new();

    private IAttack _attack;
    private IMovementStrategy _movement;

    private Rigidbody2D _rigidbody;
    private Transform _player;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = _maxHealth;
        _player = Player.Singleton.transform;

        _movement = new WalkMovement(_speed);
        _attack = new MeleeAttack(_damage, _attackRate);
    }
    private void Update()
    {
        if (Vector2.Distance(_player.position, transform.position) < 0.35f)
            _attack.Attack();
    }
    private void FixedUpdate() => _movement.Move(_rigidbody, _player);

    public void Heal(float amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

        _health += amount;
        if (_health > _maxHealth) _health = _maxHealth;
        OnHeal?.Invoke(amount);
    }
    public void Damage(float amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

        _health -= amount;
        if (_health <= 0) Die();
        OnDamage?.Invoke(amount);
    }
    public void Die()
    {
        Destroy(gameObject);
        OnDie?.Invoke();
    }
}

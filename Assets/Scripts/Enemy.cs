using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, IHealth, IEffectable
{
    public IHealth Health => this;
    public ReadOnlyCollection<TemporaryEffect> Effects => _effects.AsReadOnly();

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _health;

    public UnityEvent<float> OnHeal => _onHeal;
    public UnityEvent<float> OnDamage => _ondamage;
    public UnityEvent OnDie => _onDie;

    [SerializeField, Min(0)] private float _speed = 1;
    [SerializeField, Min(0)] private float _maxHealth = 1;
    [SerializeField, Min(0)] private float _damage = 1;
    [SerializeField, Min(0)] private float _attackRate = 1;

    private bool _dead;
    private float _health;
    private readonly UnityEvent<float> _onHeal = new();
    private readonly UnityEvent<float> _ondamage = new();
    private readonly UnityEvent _onDie = new();

    private IAttack _attack;
    private IMovementStrategy _movement;

    private Rigidbody2D _rigidbody;
    private Transform _player;

    private List<TemporaryEffect> _effects = new();

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = _maxHealth;
        _player = PlayerController.Singleton.transform;

        _movement = new WalkMovement(_speed);
        _attack = new MeleeAttack(_damage, _attackRate);
    }
    private void Update()
    {
        HandleEffects();

        if (Vector2.Distance(_player.position, transform.position) < 0.35f)
            _attack.Attack();
    }
    private void FixedUpdate() => _movement.Move(_rigidbody, _player);

    public void HandleEffects()
    {
        List<TemporaryEffect> effects = new(Effects);
        foreach (var effect in _effects)
            if (effect.IsUsing) effect.Use(this);
            else effects.Remove(effect);
        _effects = effects;
    }
    public void AddEffect(TemporaryEffect effect) => _effects.Add(effect);
    public void ClearEffects() => _effects.Clear();

    public void Heal(float amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

        OnHeal?.Invoke(amount);
        _health += amount;
        if (_health > _maxHealth) _health = _maxHealth;
    }
    public void Damage(float amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

        OnDamage?.Invoke(amount);
        _health -= amount;
        if (_health <= 0) Die();
    }
    public void Die()
    {
        if (_dead) return;
        OnDie?.Invoke();
        for (int i = 0; i < UnityEngine.Random.Range(3, 10); i++)
        {
            var money = GameWorld.MoniesPool.GetItem();
            money.transform.position = (Vector2)transform.position +
            (UnityEngine.Random.insideUnitCircle * 2);
        }
        Destroy(gameObject);
        _dead = true;
    }
}

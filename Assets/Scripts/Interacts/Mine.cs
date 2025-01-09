using System;
using UnityEngine;
using UnityEngine.Events;

public class Mine : Pickupable, IHealth
{
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _health;

    public UnityEvent<float> OnHeal => _onHeal;
    public UnityEvent<float> OnDamage => _ondamage;
    public UnityEvent OnDie => _onDie;

    [SerializeField] private GameObject _explosionVFX;
    [SerializeField, Min(0)] private float _explosionOffset = 1;
    [SerializeField, Min(0)] private float _explosionRadius = 1;
    [SerializeField, Min(0)] private float _explosionForce = 1;
    [SerializeField, Min(0)] private float _explosionDamage = 1;

    [SerializeField, Min(0)] private float _maxHealth = 1;

    private float _health;
    private readonly UnityEvent<float> _onHeal = new();
    private readonly UnityEvent<float> _ondamage = new();
    private readonly UnityEvent _onDie = new();

    private bool _explode;
    private float _explodeTime;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _health = MaxHealth;
    }
    private void Update()
    {
        if (_explode)
        {
            var value = 1 - Quad((_explodeTime - Time.time) / _explosionOffset);
            _renderer.color = Color.Lerp(Color.white, Color.red, value);
            transform.localScale = Vector3.one + Vector3.one * value / 2;
            if (Time.time > _explodeTime) DoExplosion();
        }

        static float Quad(float x) => x * x;
    }

    protected override void OnUse() => Explode();

    private void Explode()
    {
        if (_explode) return;
        _explode = true;
        _explodeTime = Time.time + _explosionOffset;
    }
    private void DoExplosion()
    {
        var colliders = new Collider2D[8];
        _ = Physics2D.OverlapCircleNonAlloc(transform.position, _explosionRadius, colliders);
        if (Vector2.Distance(transform.position,
            PlayerController.Singleton.transform.position) < _explosionRadius)
            Damage(PlayerController.Singleton.transform);
        foreach (var collider in colliders)
        {
            if (collider == null) continue;
            Damage(collider.transform);
        }
        _explode = false;

        Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);

        void Damage(Transform transform)
        {
            var direction = transform.position - base.transform.position;
            var distance = direction.magnitude;

            if (transform.TryGetComponent(out Rigidbody2D rigidbody))
            {
                float force = _explosionForce * (1 - (distance / _explosionRadius));
                rigidbody.AddForce(direction.normalized * force / Time.deltaTime);
            }
            if (transform.TryGetComponent(out IHealth health) &&
                (this is not IHealth || health != this as IHealth))
            {
                var damage = _explosionDamage * (1 - (distance / _explosionRadius));
                if (damage > 0) health.Damage(damage);
            }
        }
    }

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
        Explode();
        OnDie?.Invoke();
    }
}

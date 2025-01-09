using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public abstract class StandartBulletBase : MonoBehaviour, IBullet
{
    public UnityEvent OnHide { get; } = new();

    public Vector2 Velocity => _velocity;
    public virtual int RemainingShots { get; private set; } = 1;
    public virtual float CheckRadius { get; } = 0.05f;
    public virtual float HideTime { get; } = 10;
    public float Damage { get; private set; }
    public float Speed { get; private set; }

    protected Vector2 _velocity;
    protected IGun _gun;

    private readonly List<Collider2D> _hited = new();

    protected virtual void Update()
    {
        _velocity += Speed * (Vector2)transform.up;
        _velocity -= _velocity * 0.5f;
        transform.position += (Vector3)_velocity * Time.deltaTime;

        var hit = Physics2D.OverlapCircle(transform.position, CheckRadius);
        IGun gun;
        if (!hit || hit.isTrigger || (((gun = hit.GetComponent<IGun>()) != null) &&
            gun == _gun) || _hited.Contains(hit)) return;

        if (hit.TryGetComponent(out IHealth health)) health.Damage(Damage);
        if (hit.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.AddForceAtPosition(_velocity, transform.position);
            _velocity -= rigidbody.velocity * rigidbody.mass;
        }
        _hited.Add(hit);
        if (--RemainingShots <= 0) Hide();
    }

    public virtual void Setup(IGun gun, float speed, float damage)
    {
        _hited.Clear();
        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), HideTime);
        _gun = gun;
        Speed = speed;
        Damage = damage;
        _velocity = Vector2.zero;
    }

    private void Hide()
    {
        CancelInvoke(nameof(Hide));
        OnHide?.Invoke();
        OnHide?.RemoveAllListeners();
    }
}

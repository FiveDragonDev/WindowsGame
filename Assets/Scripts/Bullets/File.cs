using UnityEngine;
using UnityEngine.Events;

public class File : StandartBulletBase
{
    [SerializeField] private Sprite[] _sprites;

    private SpriteRenderer _renderer;

    private void Awake() => _renderer = GetComponent<SpriteRenderer>();

    public override void Setup(float speed, float damage)
    {
        base.Setup(speed, damage);
        _renderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}

public abstract class StandartBulletBase : MonoBehaviour, IBullet
{
    public UnityEvent OnDestroy { get; } = new();

    public Vector2 Velocity => _velocity;
    public int RemainingShots { get; private set; } = 1;
    public virtual float DestroyTime { get; } = 10;
    public float Damage { get; private set; }
    public float Speed { get; private set; }

    protected Vector2 _velocity;

    protected virtual void Update()
    {
        if (RemainingShots <= 0) Destroy();

        _velocity = Speed * (Vector2)transform.up;
        transform.position += (Vector3)_velocity * Time.deltaTime;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.1f);
        if (hit && !hit.isTrigger && hit.GetComponent<IGun>() == null)
        {
            if (hit.TryGetComponent(out IHealth health)) health.Damage(Damage);
            if (hit.TryGetComponent(out Rigidbody2D rigidbody))
            {
                rigidbody.AddForce(_velocity);
                Speed -= rigidbody.velocity.magnitude * rigidbody.mass;
            }
            RemainingShots--;
        }
    }

    public virtual void Setup(float speed, float damage)
    {
        Invoke(nameof(Destroy), DestroyTime);
        Speed = speed;
        Damage = damage;
    }

    protected void Destroy() => OnDestroy?.Invoke();
}

using UnityEngine;
using UnityEngine.Events;

public class File : MonoBehaviour, IBullet
{
    public UnityEvent OnDestroy { get; } = new();

    [SerializeField] private Sprite[] _sprites;

    private float _damage;
    private float _speed;

    private SpriteRenderer _renderer;

    private void Awake() => _renderer = GetComponent<SpriteRenderer>();
    private void Update()
    {
        transform.position += (Vector3)(_speed * Time.deltaTime * (Vector2)transform.up);

        RaycastHit2D hit;
        if ((hit = Physics2D.Raycast(transform.position, transform.up, 0.1f)) &&
            !hit.collider.isTrigger && hit.collider.GetComponent<IGun>() == null)
        {
            if (hit.collider.TryGetComponent(out IHealth health)) health.Damage(_damage);
            Destroy();
        }
    }

    public void Setup(float speed, float damage)
    {
        Invoke(nameof(Destroy), 10);
        _speed = speed;
        _damage = damage;
        _renderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }

    private void Destroy() => OnDestroy?.Invoke();
}

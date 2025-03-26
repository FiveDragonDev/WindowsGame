using UnityEngine;

public class RecycleBin : StandartFileGunBase
{
    [SerializeField] private Sprite _full, _empty;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _ammo = _maxAmmo;
    }
    private void Update()
    {
        if (!Picked) return;
        transform.up = Vector2.Lerp(transform.up,
            _rigidbody.velocity.normalized, Time.deltaTime * 14);
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);
    }

    protected override void OnInteract()
    {
        base.OnInteract();
        _rigidbody.freezeRotation = true;
    }
    protected override void OnThrow()
    {
        base.OnThrow();
        _rigidbody.freezeRotation = false;
    }

    public override void Shoot()
    {
        _direction = transform.up;
        base.Shoot();
        if (_ammo <= 0) _renderer.sprite = _empty;
    }
}

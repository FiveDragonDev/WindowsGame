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
        if (_rigidbody.velocity != Vector2.zero) _direction = _rigidbody.velocity.normalized;
        if (Picked)
        {
            transform.up = Vector2.Lerp(transform.up, _direction, Time.deltaTime * 14);
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);
        }
    }

    protected override void OnUse()
    {
        if (Time.time > _nextShotTime) Shoot();
    }

    public override void Shoot()
    {
        base.Shoot();
        if (_ammo <= 0) _renderer.sprite = _empty;
    }
}

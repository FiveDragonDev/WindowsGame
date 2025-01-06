using UnityEngine;

public class Directory : StandartFileGunBase
{
    [SerializeField] private Sprite _full, _empty, _closed;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _ammo = _maxAmmo;
    }
    private void Update()
    {
        if (Picked && Time.time > _nextShotTime) Shoot();
    }

    public override void Interact()
    {
        base.Interact();
        if (_ammo > 0) _renderer.sprite = _full;
        else _renderer.sprite = _empty;
    }
    public override void Throw()
    {
        base.Throw();
        _renderer.sprite = _closed;
    }

    public override void Shoot()
    {
        base.Shoot();
        if (_ammo <= 0) _renderer.sprite = _empty;
    }
}

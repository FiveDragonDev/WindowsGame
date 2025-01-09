using UnityEngine;

public class Directory : StandartFileGunBase
{
    [SerializeField] private Sprite _full, _empty;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _ammo = _maxAmmo;
    }

    protected override void OnInteract()
    {
        if (_ammo > 0) _renderer.sprite = _full;
        else _renderer.sprite = _empty;
    }

    public override void Shoot()
    {
        base.Shoot();
        if (_ammo <= 0) _renderer.sprite = _empty;
    }
}

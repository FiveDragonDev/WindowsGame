using UnityEngine;

public class Directory : Pickupable, IGun
{
    public int MaxAmmo => _maxAmmo;
    public int Ammo => _ammo;
    public float FireRate => _fireRate;
    public float DamagePerBullet => _damage;
    public IBullet Bullet => GameWorld.FilesPool.Prefab.GetComponent<IBullet>();
    public int BulletsPerShot => _bulletsPerShot;
    public float Accuracy => _accuracy;

    [SerializeField] private Sprite _full, _empty, _closed;
    [SerializeField] private GameObject _filesFX;
    [SerializeField, Min(0)] private float _fileSpeed = 1;
    [SerializeField, Min(0)] private float _damage = 1;
    [SerializeField, Min(0)] private float _fireRate = 1;
    [SerializeField, Min(1)] private int _bulletsPerShot = 1;
    [SerializeField, Min(0)] private float _accuracy = 1;
    [SerializeField, Min(1)] private int _maxAmmo = 1;

    private int _ammo;
    private float _nextShotTime;
    private Vector2 _direction;
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

    public void Shoot()
    {
        if (_ammo <= 0 || _bulletsPerShot <= 0) return;

        Vector2 recoilForce = Vector2.zero;
        if (_rigidbody.velocity != Vector2.zero) _direction = _rigidbody.velocity.normalized;
        for (int i = 0; i < BulletsPerShot; i++)
        {
            var file = GameWorld.FilesPool.GetItem();
            file.transform.position = transform.position;
            file.transform.up = (_direction + Random.insideUnitCircle / Accuracy).normalized;
            var speed = _fileSpeed + _rigidbody.velocity.magnitude / 5;
            var damage = DamagePerBullet + _rigidbody.velocity.magnitude / 5;
            var fileComponent = file.GetComponent<File>();
            fileComponent.Setup(speed, damage);
            fileComponent.OnDestroy.AddListener(() => GameWorld.FilesPool.Release(file));
            recoilForce += speed / 2 * (Vector2)file.transform.up;
        }
        _rigidbody.velocity += recoilForce / _bulletsPerShot;
        Instantiate(_filesFX, transform.position, Quaternion.LookRotation(transform.up));
        _nextShotTime = Time.time + 1f / FireRate;
        _ammo--;
        if (_ammo <= 0) _renderer.sprite = _empty;
    }
}

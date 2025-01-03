using UnityEngine;

public class RecycleBin : Pickupable, IGun
{
    public int MaxAmmo => _maxAmmo;
    public int Ammo => _ammo;
    public float FireRate => _fireRate;
    public float DamagePerBullet => _damage;
    public IBullet Bullet => _file;
    public int BulletsPerShot => _bulletsPerShot;
    public float Accuracy => _accuracy;

    [SerializeField] private Sprite _full, _empty;
    [SerializeField] private File _file;
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

    private GameObjectPool _pool;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _pool = new(_file.gameObject, 0);
        _ammo = _maxAmmo;
    }
    private void Update()
    {
        if (_rigidbody.velocity != Vector2.zero) _direction = _rigidbody.velocity.normalized;
        if (Picked)
        {
            transform.up = Vector2.Lerp(transform.up, _direction, Time.deltaTime * 14);
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);
            if (Time.time > _nextShotTime) Shoot();
        }
    }

    public void Shoot()
    {
        if (_ammo <= 0 || _bulletsPerShot <= 0) return;

        Vector2 recoilForce = Vector2.zero;
        for (int i = 0; i < BulletsPerShot; i++)
        {
            var file = _pool.GetItem();
            file.transform.position = transform.position;
            file.transform.up = ((Vector2)transform.up +
                Random.insideUnitCircle / Accuracy).normalized;
            var speed = _fileSpeed + _rigidbody.velocity.magnitude / 5;
            var damage = DamagePerBullet + _rigidbody.velocity.magnitude / 5;
            var fileComponent = file.GetComponent<File>();
            fileComponent.Setup(speed, damage);
            fileComponent.OnDestroy.AddListener(() => _pool.Release(file));
            recoilForce += speed / 2 * (Vector2)file.transform.up;
        }
        _rigidbody.velocity += recoilForce / _bulletsPerShot;
        Instantiate(_filesFX, transform.position, Quaternion.LookRotation(transform.up));
        _nextShotTime = Time.time + 1f / FireRate;
        _ammo--;
        if (_ammo <= 0) _renderer.sprite = _empty;
    }
}

using UnityEngine;

public interface IGun
{
    public IBullet Bullet { get; }
    public float DamagePerBullet { get; }
    public float BulletSpeed { get; }

    public void Shoot();
}
public abstract class StandartFileGunBase : Pickupable, IGun
{
    public virtual int MaxAmmo => _maxAmmo;
    public virtual int Ammo => _ammo;
    public virtual float FireRate => _fireRate;
    public virtual float DamagePerBullet => _damage;
    public float BulletSpeed => _fileSpeed;
    public virtual IBullet Bullet => GameWorld.FilesPool.Prefab.GetComponent<IBullet>();
    public virtual int BulletsPerShot => _bulletsPerShot;
    public virtual float Accuracy => _accuracy;

    [SerializeField] protected GameObject _filesFX;
    [SerializeField, Min(0)] protected float _fileSpeed = 1;
    [SerializeField, Min(0)] protected float _damage = 1;
    [SerializeField, Min(0)] protected float _fireRate = 1;
    [SerializeField, Min(1)] protected int _bulletsPerShot = 1;
    [SerializeField, Min(0)] protected float _accuracy = 1;
    [SerializeField, Min(1)] protected int _maxAmmo = 1;

    protected int _ammo;
    protected float _nextShotTime;
    protected Vector2 _direction;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _ammo = MaxAmmo;
    }

    protected override void OnUse() => Shoot();
    protected override void OnPointerUse() => Shoot();

    public virtual void Shoot()
    {
        if (Time.time <= _nextShotTime ||
            _ammo <= 0 || _bulletsPerShot <= 0) return;

        Vector2 recoilForce = Vector2.zero;
        for (int i = 0; i < BulletsPerShot; i++)
        {
            var file = GameWorld.FilesPool.GetItem();
            file.transform.position = transform.position;
            file.transform.up = (_direction + Random.insideUnitCircle / Accuracy).normalized;
            var speed = _fileSpeed + _rigidbody.velocity.magnitude / 5;
            var damage = DamagePerBullet + _rigidbody.velocity.magnitude / 5;
            var fileComponent = file.GetComponent<File>();
            fileComponent.Setup(this, speed, damage);
            fileComponent.OnHide.AddListener(() => GameWorld.FilesPool.Release(file));
            recoilForce += speed / 2 * (Vector2)file.transform.up;
        }
        _rigidbody.velocity -= recoilForce / BulletsPerShot;
        Instantiate(_filesFX, transform.position, Quaternion.LookRotation(transform.up));
        _nextShotTime = Time.time + 1f / FireRate;
        _ammo--;
    }
}

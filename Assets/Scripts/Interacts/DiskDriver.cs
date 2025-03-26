using UnityEngine;

public class DiskDriver : Pickupable, IGun
{
    public virtual float FireRate => _fireRate;
    public virtual float DamagePerBullet => _damage;
    public float BulletSpeed => _speed;
    public virtual IBullet Bullet => GameWorld.DiskPool.Prefab.GetComponent<IBullet>();
    public virtual int BulletsPerShot => _bulletsPerShot;
    public virtual float Accuracy => _accuracy;

    [SerializeField, Min(0)] protected float _speed = 1;
    [SerializeField, Min(0)] protected float _damage = 1;
    [SerializeField, Min(0)] protected float _fireRate = 1;
    [SerializeField, Min(1)] protected int _bulletsPerShot = 1;
    [SerializeField, Min(0)] protected float _accuracy = 1;

    protected float _nextShotTime;
    protected Vector2 _direction;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void OnUse() => Shoot();
    protected override void OnPointerUse() => Shoot();

    public virtual void Shoot()
    {
        if (Time.time <= _nextShotTime || _bulletsPerShot <= 0) return;

        Vector2 recoilForce = Vector2.zero;
        for (int i = 0; i < BulletsPerShot; i++)
        {
            var bullet = GameWorld.DiskPool.GetItem();
            bullet.transform.position = transform.position;
            bullet.transform.up = (_direction + Random.insideUnitCircle / Accuracy).normalized;
            var speed = _speed + _rigidbody.velocity.magnitude / 5;
            var damage = DamagePerBullet + _rigidbody.velocity.magnitude / 5;
            var bulletComponent = bullet.GetComponent<Disk>();
            bulletComponent.Setup(this, speed, damage);
            bulletComponent.OnHide.AddListener(() => GameWorld.DiskPool.Release(bullet));
            recoilForce += speed / 2 * (Vector2)bullet.transform.up;
        }
        _rigidbody.velocity -= recoilForce / BulletsPerShot;
        _nextShotTime = Time.time + 1f / FireRate;
    }
}

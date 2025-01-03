public interface IGun
{
    public int MaxAmmo { get; }
    public int Ammo { get; }
    public float FireRate { get; }
    public float DamagePerBullet { get; }
    public int BulletsPerShot { get; }
    public float Accuracy { get; }
    public IBullet Bullet { get; }

    public void Shoot();
}

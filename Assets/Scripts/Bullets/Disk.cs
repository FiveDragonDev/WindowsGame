using UnityEngine;

public class Disk : StandartBulletBase
{
    public override float CheckRadius => 0.4f;
    public override int RemainingShots => 5;

    private void Start()
    {
        OnHit.AddListener((c) => Bounce(c));
    }

    private void Bounce(Collider2D hit)
    {
        var direction = (hit.transform.position - transform.position).normalized;
        _velocity = direction * _velocity.magnitude / 2;
    }
}

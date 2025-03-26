using UnityEngine;

public class Cookie : Pickupable
{
    [SerializeField, Min(0)] private float _damagePerClick = 1;

    protected override void OnPointerUse()
    {
        var healths = UnityUtils.GetObjectsByType<Enemy>();
        if (healths.Length < 1) return;

        IHealth health;
        do
        {
            health = healths[Random.Range(0, healths.Length)];
        } while (health == (PlayerHealth.Singleton as IHealth));
        health.Damage(_damagePerClick);
    }
}

using UnityEngine;

public class RegenerationEffect : TemporaryEffect
{
    public float HealPerSecond { get; }

    public RegenerationEffect(float duration, float healAmount) :
        base(duration) => HealPerSecond = healAmount;

    public override void Use(IEffectable effectable) =>
        effectable.Health.Heal(HealPerSecond * Time.deltaTime);
}

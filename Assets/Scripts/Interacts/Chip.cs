using UnityEngine;

public sealed class Chip : EffectSource
{
    [SerializeField, Min(0)] private float _healAmount = 1;
    [SerializeField, Min(0)] private float _healDuration = 1;

    private void Start()
    {
        _effect = new RegenerationEffect(_healDuration, _healAmount);
        _effect.OnApply.AddListener(() => Destroy(gameObject));
    }

    protected override void OnUse() => _effect.Apply(PlayerController.Singleton);
    protected override void OnPointerUse() => _effect.Apply(PlayerController.Singleton);
    protected override void OnEnter(IEffectable effectable)
    {
        if (effectable == PlayerController.Singleton as IEffectable) return;
        _effect.Apply(effectable);
    }
}

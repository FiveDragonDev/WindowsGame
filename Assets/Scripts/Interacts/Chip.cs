using UnityEngine;

public class Chip : EffectSource
{
    [SerializeField, Min(0)] private float _healAmount = 1;
    [SerializeField, Min(0)] private float _healDuration = 1;

    private void Start()
    {
        _effect = new RegenerationEffect(_healDuration, _healAmount);
        _effect.OnApply.AddListener(() => Destroy(gameObject));
    }
}

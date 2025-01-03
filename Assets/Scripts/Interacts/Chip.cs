using UnityEngine;

public class Chip : EffectSource
{
    [SerializeField, Min(0)] private float _healAmount = 1;

    private void Start()
    {
        _effect = new HealEffect(_healAmount);
        _effect.OnApply.AddListener(() => Destroy(gameObject));
    }

    public override void Interact()
    {
        Effect.Apply();
        OnInteract?.Invoke();
    }
}

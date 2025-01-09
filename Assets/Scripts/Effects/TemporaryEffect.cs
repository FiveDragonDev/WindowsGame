using UnityEngine;
using UnityEngine.Events;

public abstract class TemporaryEffect : IEffect
{
    public UnityEvent OnApply => _onApply;
    public bool IsUsing => Time.time < _stopTime;

    public float Duration { get; }

    protected float _stopTime;

    private readonly UnityEvent _onApply = new();

    public TemporaryEffect(float duration) => Duration = duration;

    public virtual void Apply(IEffectable other)
    {
        OnApply?.Invoke();
        _stopTime = Time.time + Duration;
        other.AddEffect(this);
    }
    public abstract void Use(IEffectable effectable);
}

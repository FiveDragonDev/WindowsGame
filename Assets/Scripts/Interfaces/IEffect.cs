using UnityEngine.Events;

public interface IEffect
{
    public UnityEvent OnApply { get; }

    public virtual void Apply() => OnApply?.Invoke();
}

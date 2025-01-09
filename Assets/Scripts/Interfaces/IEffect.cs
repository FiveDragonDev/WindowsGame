using UnityEngine;
using UnityEngine.Events;

public interface IEffect
{
    public UnityEvent OnApply { get; }

    public void Apply(IEffectable body);
}

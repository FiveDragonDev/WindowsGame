using UnityEngine;
using UnityEngine.Events;

public abstract class EffectSource : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteract => _onInteract;
    public IEffect Effect => _effect;

    [SerializeReference] protected IEffect _effect;

    private readonly UnityEvent _onInteract = new();

    public virtual void Interact()
    {
        Effect.Apply();
        OnInteract?.Invoke();
    }
}

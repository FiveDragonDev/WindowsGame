using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Pickupable : CollisionDamage, IInteractable
{
    public UnityEvent OnInteractEvent => _onInteractEvent;
    public UnityEvent OnUseEvent => _onUseEvent;
    public UnityEvent OnThrowEvent => _onThrowEvent;

    public virtual bool CanPickup { get; protected set; } = true;
    public bool Picked { get; private set; }

    private readonly UnityEvent _onInteractEvent = new();
    private readonly UnityEvent _onUseEvent = new();
    private readonly UnityEvent _onThrowEvent = new();

    public void Interact()
    {
        OnInteract();
        OnInteractEvent?.Invoke();
        Picked = true;
    }
    public void Use()
    {
        OnUse();
        OnUseEvent?.Invoke();
    }

    public void Throw()
    {
        OnThrow();
        OnThrowEvent?.Invoke();
        Picked = false;
    }

    protected virtual void OnUse() { }
    protected virtual void OnInteract() { }
    protected virtual void OnThrow() { }
}

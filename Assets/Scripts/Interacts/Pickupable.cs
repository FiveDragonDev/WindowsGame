using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Pickupable : MonoBehaviour, IInteractable
{
    public bool CanPickup { get; protected set; } = true;
    public bool Picked { get; private set; }

    public UnityEvent OnInteract => _onInteract;
    public UnityEvent OnThrow => _onThrow;

    private readonly UnityEvent _onInteract = new();
    private readonly UnityEvent _onThrow = new();

    protected Rigidbody2D _rigidbody;

    private void Start() => _rigidbody = GetComponent<Rigidbody2D>();
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        float damage = _rigidbody.velocity.magnitude / 10;
        if (other.rigidbody) damage += other.rigidbody.velocity.magnitude / 10;

        if (TryGetComponent(out IHealth thisHealth)) thisHealth.Damage(damage);
        if (other.collider.TryGetComponent(out IHealth otherHealth)) otherHealth.Damage(damage);
    }

    public virtual void Interact()
    {
        OnInteract?.Invoke();
        Picked = true;
    }
    public virtual void Throw()
    {
        OnThrow?.Invoke();
        Picked = false;
    }
}

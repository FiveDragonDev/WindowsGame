using UnityEngine.Events;

public interface IInteractable
{
    public UnityEvent OnInteractEvent { get; }

    public void Interact() { }
    public void Use() { }
    public void PointerUse() { }
}

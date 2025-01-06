using UnityEngine;
using UnityEngine.Events;

public sealed class Door : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteract => _onInteract;
    public bool NeedKey => _needKey && !_open;

    [SerializeField] private bool _needKey;

    private bool _open;
    private readonly UnityEvent _onInteract = new();

    private void Start() => _open = !_needKey;

    public void Open() => _open = true;

    public void Interact()
    {
        if (!_open) return;
        OnInteract?.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;

public sealed class Door : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteractEvent => _onInteractEvent;

    public bool Locked => _useKey && !_open;

    [SerializeField] private bool _useKey;
    [SerializeField] private GameObject _padlock;

    private bool _open;

    private readonly UnityEvent _onInteractEvent = new();

    private void Start()
    {
        _open = !_useKey;
        _padlock.SetActive(Locked);
    }

    public void Open()
    {
        _open = true;
        _padlock.SetActive(Locked);
    }

    public void Interact()
    {
        if (!_open) return;
        OnInteractEvent?.Invoke();
    }

    public void OnInteract() => throw new System.NotImplementedException();
}

using TMPro;
using UnityEngine;
using UnityEngine.Events;

public sealed class FlowNumbers : MonoBehaviour
{
    public UnityEvent OnHide => _onHide;

    [SerializeField] private TextMeshPro _textMesh;

    private Animator _animator;
    private readonly UnityEvent _onHide = new();

    private void Awake() => _animator = GetComponent<Animator>();

    public void Initialize(float digits)
    {
        _animator.SetTrigger("Show");
        _textMesh.text = digits.ToString("0.0");
    }

    public void Hide() => OnHide?.Invoke();
}

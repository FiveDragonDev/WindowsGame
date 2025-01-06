using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Spring : MonoBehaviour
{
    public float SpringForce => Stiffness * Distance;
    public float Distance => Vector2.Distance(Position, ConnectedRigidbody.position);
    public Vector2 Position => _rigidbody.position + Anchor;

    public UnityEvent OnBreak => _onBreak;

    public float BreakForce
    {
        get => _breakForce;
        set => _breakForce = Mathf.Max(value, 0);
    }
    public float Stiffness
    {
        get => _stiffness;
        set => _stiffness = Mathf.Max(value, 0);
    }
    public float Damping
    {
        get => _damping;
        set => _damping = Mathf.Max(value, 0);
    }
    public Vector2 Anchor
    {
        get => _anchor;
        set => _anchor = value;
    }
    public Rigidbody2D ConnectedRigidbody
    {
        get => _connectedRigidbody;
        set => _connectedRigidbody = value;
    }

    [SerializeField, Min(0)] private float _breakForce = float.PositiveInfinity;
    [SerializeField, Min(0)] private float _stiffness = 1;
    [SerializeField, Range(0, 1)] private float _damping = 1;
    [SerializeField] private Vector2 _anchor = Vector2.zero;
    [SerializeField] private Rigidbody2D _connectedRigidbody;

    private readonly UnityEvent _onBreak = new();
    private Rigidbody2D _rigidbody;

    private void Start() => _rigidbody = GetComponent<Rigidbody2D>();
    private void FixedUpdate()
    {
        if (ConnectedRigidbody == null) return;
        var force = SpringForce;
        if (force > BreakForce)
        {
            OnBreak?.Invoke();
            Destroy(this);
        }

        var direction = (ConnectedRigidbody.position - Position).normalized;
        _rigidbody.AddForceAtPosition((direction * force) -
            _rigidbody.velocity * Damping, Position);
    }
}

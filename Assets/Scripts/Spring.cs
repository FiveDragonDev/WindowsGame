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
    public float MinDistance
    {
        get => _minDistance;
        set => _minDistance = Mathf.Max(value, 0);
    }
    public float Damping
    {
        get => _damping;
        set => _damping = Mathf.Max(value, 0);
    }
    public float ConnectedRigidbodyDependence
    {
        get => _connectedRigidbodyDependence;
        set => _connectedRigidbodyDependence = Mathf.Clamp01(value);
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
    [SerializeField, Min(0)] private float _minDistance = 0;
    [SerializeField, Min(0)] private float _damping = 1;
    [SerializeField, Range(0, 1)] private float _connectedRigidbodyDependence = 1;
    [SerializeField] private Vector2 _anchor = Vector2.zero;
    [SerializeField] private Rigidbody2D _connectedRigidbody;

    private readonly UnityEvent _onBreak = new();
    private Rigidbody2D _rigidbody;

    private void Start() => _rigidbody = GetComponent<Rigidbody2D>();
    private void FixedUpdate()
    {
        if (_connectedRigidbody == null || Distance < MinDistance) return;
        var direction = (ConnectedRigidbody.position - Position).normalized;
        ApplyForces(direction);
    }
    private void ApplyForces(Vector2 direction)
    {
        var springForce = SpringForce * direction;
        var dampingForce = Damping * _rigidbody.velocity;

        if ((springForce - dampingForce).magnitude / 2 > _breakForce)
        {
            OnBreak?.Invoke();
            Destroy(this);
            return;
        }

        _rigidbody.AddForce((springForce - dampingForce) / 2);
        ConnectedRigidbody.AddForce((-springForce - dampingForce) *
            _connectedRigidbodyDependence / 2);

        ApplyTorque(direction);
    }
    private void ApplyTorque(Vector2 direction)
    {
        var contactPoint = GetContactPoint(1f, Mathf.Atan2(direction.y, direction.x));
        var leverArm = contactPoint - _rigidbody.position;
        var torque = leverArm.x * _rigidbody.velocity.y - leverArm.y * _rigidbody.velocity.x;

        _rigidbody.AddTorque(torque);
    }
    private Vector2 GetContactPoint(float radius, float angle) =>
        (Vector2)transform.TransformPoint(new(radius *
            Mathf.Cos(angle), radius * Mathf.Sin(angle)));
}

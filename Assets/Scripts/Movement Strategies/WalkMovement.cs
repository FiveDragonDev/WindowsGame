using UnityEngine;

public sealed class WalkMovement : IMovementStrategy
{
    public float Speed { get => _speed; set => _speed = value; }

    private float _speed;
    private Vector2 _acceleration;

    public WalkMovement(float speed) => _speed = speed;

    public void Move(Rigidbody2D rigidbody, Transform target)
    {
        var direction = ((Vector2)target.position - rigidbody.position).normalized;
        var distance = ((Vector2)target.position - rigidbody.position).magnitude;

        _acceleration -= _acceleration * 0.5f;
        if (distance < 0.1f) return;
        _acceleration += Speed * Time.deltaTime * direction;
        rigidbody.velocity += _acceleration * Time.deltaTime;
    }
}

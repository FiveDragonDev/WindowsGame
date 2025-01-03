using UnityEngine;

public sealed class WalkMovement : IMovementStrategy
{
    public float Speed { get => _speed; set => _speed = value; }

    private float _speed;

    public WalkMovement(float speed) => _speed = speed;

    public void Move(Rigidbody2D rigidbody, Transform target)
    {
        var direction = ((Vector2)target.position - rigidbody.position).normalized;
        var distance = ((Vector2)target.position - rigidbody.position).magnitude;

        if (distance < 0.1f) return;
        rigidbody.velocity += Speed * Time.fixedDeltaTime * direction;
    }
}

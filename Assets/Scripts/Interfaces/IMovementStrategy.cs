using UnityEngine;

public interface IMovementStrategy
{
    public float Speed { get; set; }

    public void Move(Rigidbody2D rigidbody, Transform target);
}

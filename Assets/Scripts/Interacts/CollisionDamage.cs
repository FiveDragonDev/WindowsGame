using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CollisionDamage : MonoBehaviour
{

    [SerializeField, Min(0)] protected float _collisionDamageMultiplier = 1;

    protected Rigidbody2D _rigidbody;

    private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        var thisImpulse = _rigidbody.velocity * _rigidbody.mass;
        var otherImpulse = other.relativeVelocity;
        if (other.rigidbody != null) otherImpulse *= other.rigidbody.mass;
        var relativeImpulse = thisImpulse - otherImpulse;
        float collisionSpeed = relativeImpulse.magnitude;
        float damage = collisionSpeed * _collisionDamageMultiplier / 10;

        if (TryGetComponent(out IHealth health)) health.Damage(damage);
        if (other.collider.TryGetComponent(out health)) health.Damage(damage);
    }
}

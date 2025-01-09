using UnityEngine;

public sealed class Key : Pickupable
{
    private void Use(Door door)
    {
        door.Open();
        Destroy(gameObject);
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.transform.TryGetComponent(out Door door) && door.Locked) Use(door);
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Singleton { get; private set; }

    [SerializeField, Min(0)] private float _bulletSpeed = 1;
    [SerializeField, Min(0)] private float _bulletDamage = 1;

    private Spring _joint;
    private Pickupable _pickupable;
    private RaycastHit2D _hit;

    private Camera _camera;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }
    private void Update()
    {
        transform.position = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !_pickupable)
        {
            _hit = Physics2D.Raycast(transform.position, Vector3.forward);
            if (_hit && !_hit.collider.isTrigger &&
                _hit.collider.TryGetComponent(out IInteractable interactable))
                Interact(interactable);
            else Shoot();
        }
        else if (Input.GetMouseButtonUp(0) && _pickupable) Throw();
    }

    private void Interact(IInteractable interactable)
    {
        if ((_pickupable = interactable as Pickupable) && _pickupable.CanPickup) PickUp();
        interactable.Interact();
    }

    private void PickUp()
    {
        _joint = _pickupable.gameObject.AddComponent<Spring>();
        _joint.ConnectedRigidbody = _rigidbody;
        _joint.Anchor = transform.position - _pickupable.transform.position;
        _joint.BreakForce = 900;
        _joint.Stiffness = 300;
        _joint.Damping = 7;
        _joint.OnBreak.AddListener(Throw);
    }
    private void Throw()
    {
        Destroy(_joint);
        _pickupable.Throw();
        _joint = null;
        _pickupable = null;
    }

    private void Shoot()
    {
        var cursor = GameWorld.MiniCursorsPool.GetItem().GetComponent<MiniCursor>();
        cursor.transform.position = transform.position;
        var enemy = UnityUtils.GetClosestObjectByType<Enemy>();
        var direction = Vector2.up;
        if (enemy) direction = (enemy.transform.position - transform.position).normalized;
        cursor.transform.up = direction;
        cursor.Setup(_bulletSpeed, _bulletDamage);
        cursor.OnDestroy.AddListener(() => GameWorld.MiniCursorsPool.Release(cursor.gameObject));
    }

}

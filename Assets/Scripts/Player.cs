using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Singleton { get; private set; }

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
        /*if (_joint)
            _joint.transform.rotation = Quaternion.Lerp(_joint.transform.rotation,
            Quaternion.Euler(16 * Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * Vector3.back),
            Time.deltaTime * 8);*/

        _hit = Physics2D.Raycast(transform.position, Vector3.forward);
        if (_hit && !_hit.collider.isTrigger && !_joint &&
            _hit.collider.TryGetComponent(out IInteractable interactable))
        {
            if (Input.GetMouseButtonDown(0)) Interact(interactable);
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
}

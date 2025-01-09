using UnityEngine;

public sealed class MorningStar : MonoBehaviour
{
    [SerializeField] private Pickupable _handle;
    [SerializeField] private CollisionDamage _star;
    [SerializeField] private Spring _joint;
    [SerializeField] private LineRenderer _renderer;

    private void Awake()
    {
        _handle.GetComponent<Rigidbody2D>().mass = 2.5f;

        OnThrow();

        _handle.OnInteractEvent.AddListener(OnInteract);
        _handle.OnThrowEvent.AddListener(OnThrow);
        _joint.OnBreak.AddListener(OnBreak);

        _renderer.positionCount = 2;
    }
    private void Update()
    {
        if (_renderer == null) return;
        _renderer.widthMultiplier = 1.5f / (Vector2.Distance(
            _handle.transform.position, _star.transform.position) + 0.75f);
        _renderer.SetPosition(0, _handle.transform.position);
        _renderer.SetPosition(1, _star.transform.position);
    }

    private void OnInteract() => _joint.ConnectedRigidbodyDependence = 0.3f;
    private void OnThrow() => _joint.ConnectedRigidbodyDependence = 0;
    private void OnBreak()
    {
        _handle.GetComponent<Rigidbody2D>().mass = 0.66f;
        _star.gameObject.AddComponent<Pickupable>();
        Destroy(_joint);
        Destroy(_star);
        Destroy(_renderer);
    }
}

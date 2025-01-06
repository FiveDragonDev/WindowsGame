using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour, IGun
{
    public static Player Singleton { get; private set; }

    public int Monies { get; private set; }

    public IBullet Bullet => GameWorld.MiniCursorsPool.Prefab.GetComponent<IBullet>();
    public float DamagePerBullet => _bulletDamage;
    public float BulletSpeed => _bulletSpeed;

    [SerializeField, Min(0)] private float _bulletSpeed = 1;
    [SerializeField, Min(0)] private float _bulletDamage = 1;
    [SerializeField] private TextMeshProUGUI _moniesText;

    private readonly List<TemporaryEffect> _effects = new();

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
        AddMonies(0);
    }
    private void Update()
    {
        foreach (var effect in _effects)
            if (effect.IsUsing) effect.Use();

        transform.position = _camera.ScreenToWorldPoint(Input.mousePosition);

        var money = UnityUtils.GetClosestObjectByType<Money>();
        if (money != null && Vector3.Distance(transform.position,
            money.transform.position) < 0.2f)
        {
            AddMonies(1);
            money.Destroy();
        }

        if (Input.GetMouseButtonDown(0) && !_pickupable)
        {
            _hit = Physics2D.Raycast(transform.position, Vector3.forward);
            if (_hit && !_hit.collider.isTrigger)
            {
                if (_hit.collider.TryGetComponent(out IInteractable interactable))
                    Interact(interactable);
            }
            else Shoot();
        }
        else if (Input.GetMouseButtonUp(0) && _pickupable) Throw();
    }

    public void AddMonies(int amount)
    {
        Monies += amount;
        _moniesText.text = Monies.ToString("000000");
    }
    public void AddEffect(TemporaryEffect effect) => _effects.Add(effect);

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
        _joint.Stiffness = 600;
        _joint.Damping = 18;
        _joint.OnBreak.AddListener(Throw);
    }
    private void Throw()
    {
        Destroy(_joint);
        _pickupable.Throw();
        _joint = null;
        _pickupable = null;
    }

    public void Shoot()
    {
        var cursor = GameWorld.MiniCursorsPool.GetItem().GetComponent<MiniCursor>();
        cursor.transform.position = transform.position;
        var enemy = UnityUtils.GetClosestObjectByType<Enemy>();
        var direction = Vector2.up;
        if (enemy) direction = (enemy.transform.position - transform.position).normalized;
        cursor.transform.up = direction;
        cursor.Setup(this, _bulletSpeed, _bulletDamage);
        cursor.OnHide.AddListener(() => GameWorld.MiniCursorsPool.Release(cursor.gameObject));
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;

public sealed class PlayerController : MonoBehaviour, IGun, IEffectable
{
    public static PlayerController Singleton { get; private set; }

    public IHealth Health => PlayerHealth.Singleton;
    public ReadOnlyCollection<TemporaryEffect> Effects => _effects.AsReadOnly();

    public int Monies { get; private set; }

    public IBullet Bullet => GameWorld.MiniCursorsPool.Prefab.GetComponent<IBullet>();
    public float DamagePerBullet => _bulletDamage;
    public float BulletSpeed => _bulletSpeed;

    [SerializeField, Min(0)] private float _bulletSpeed = 1;
    [SerializeField, Min(0)] private float _bulletDamage = 1;
    [SerializeField] private TextMeshProUGUI _moniesText;

    private List<TemporaryEffect> _effects = new();

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
        _moniesText.text = Monies.ToString();
    }
    private void Update()
    {
        HandleEffects();

        transform.position = _camera.ScreenToWorldPoint(Input.mousePosition);

        var money = UnityUtils.GetClosestObjectByType<Money>();
        if (money != null && Vector3.Distance(transform.position,
            money.transform.position) < 0.75f)
        {
            AddMonies(1);
            money.Destroy();
        }

        if (!_pickupable)
        {
            _hit = Physics2D.Raycast(transform.position, Vector3.forward);
            if (_hit && !_hit.collider.isTrigger)
            {
                if (_hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (Input.GetMouseButtonDown(0)) Interact(interactable);
                    if (Input.GetMouseButtonDown(1)) Use(interactable);
                }
            }
            else if (Input.GetMouseButtonDown(1)) Shoot();
        }
        else
        {
            if (Input.GetMouseButtonUp(0)) Throw();
            if (Input.GetMouseButton(1)) Use(_pickupable);
        }
    }

    public void AddMonies(int amount)
    {
        Monies += amount;
        _moniesText.text = Monies.ToString();
    }
    public void HandleEffects()
    {
        List<TemporaryEffect> effects = new(Effects);
        foreach (var effect in _effects)
            if (effect.IsUsing) effect.Use(this);
            else effects.Remove(effect);
        _effects = effects;
    }
    public void AddEffect(TemporaryEffect effect) => _effects.Add(effect);
    public void ClearEffects() => _effects.Clear();

    private void Use(IInteractable interactable) => interactable.Use();
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
        _joint.BreakForce = 1000;
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
        var direction = Vector2.up;
        var enemy = UnityUtils.GetClosestObjectByType<Enemy>();
        if (enemy) direction = (enemy.transform.position - transform.position).normalized;
        cursor.transform.up = direction;
        cursor.Setup(this, _bulletSpeed, _bulletDamage);
        cursor.OnHide.AddListener(() => GameWorld.MiniCursorsPool.Release(cursor.gameObject));
    }
}

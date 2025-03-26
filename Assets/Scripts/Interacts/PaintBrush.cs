using UnityEngine;

public class PaintBrush : Pickupable
{
    [SerializeField, Min(0)] private float _trailDamage = 1;
    [SerializeField, Min(0)] private float _damageInterval = 1;

    private TrailRenderer _trail;
    private float _nextDamageTime;
    private readonly RaycastHit2D[] _hits = new RaycastHit2D[5];

    private void Start()
    {
        _trail = GetComponent<TrailRenderer>();
        _trail.enabled = false;
        _nextDamageTime = Time.time + _damageInterval;
    }
    private void Update()
    {
        if (!Picked || _trail.time <= 0 ||
            Time.time < _nextDamageTime) return;
        ApplyDamageToObjectsOnTrail();
    }

    private void ApplyDamageToObjectsOnTrail()
    {
        if (_trail.positionCount < 1) return;
        for (int i = 0; i < _trail.positionCount - 1; i++)
        {
            var start = _trail.GetPosition(i);
            var end = _trail.GetPosition(i + 1);
            if (Physics2D.CircleCastNonAlloc(start, _trail.widthCurve.Evaluate(0.5f),
                (end - start).normalized, _hits, Vector2.Distance(start, end)) < 1) continue;

            foreach (var hit in _hits)
            {
                if (hit && hit.collider.TryGetComponent(out IHealth health))
                {
                    health.Damage(_trailDamage);
                    _nextDamageTime = Time.time + _damageInterval;
                }
            }
        }
    }

    protected override void OnInteract() => _trail.enabled = true;
    protected override void OnThrow()
    {
        _trail.Clear();
        _trail.enabled = false;
    }
}

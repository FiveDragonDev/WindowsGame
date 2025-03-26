using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Pen : Pickupable
{
    [SerializeField, Min(0)] private float _damage = 1;
    [SerializeField, Min(0)] private float _damageInterval = 1;

    private float _nextDamageTime;

    private readonly List<Vector3> _points = new();
    private readonly RaycastHit2D[] _hits = new RaycastHit2D[5];
    private LineRenderer _line;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Time.time > _nextDamageTime) Attack();

        if (_points.Contains(transform.position) || _points.Count > 0 &&
            Vector3.Distance(transform.position, _points[^1]) <= 0.05f) return;
        _points.Add(transform.position);
        _line.positionCount = _points.Count;
        _line.SetPositions(_points.ToArray());
    }

    private void Attack()
    {
        if (_points.Count < 1) return;
        for (int i = 0; i < _points.Count - 1; i++)
        {
            var start = _points[i];
            var end = _points[i + 1];
            if (Physics2D.CircleCastNonAlloc(start, _line.widthCurve.Evaluate(0.5f),
                (end - start).normalized, _hits, Vector2.Distance(start, end)) < 1) continue;

            foreach (var hit in _hits)
            {
                if (hit && hit.collider.TryGetComponent(out IHealth health))
                {
                    health.Damage(_damage);
                    _nextDamageTime = Time.time + _damageInterval;
                }
            }
        }
    }
}

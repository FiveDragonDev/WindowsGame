using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Pen : Pickupable
{
    private readonly List<Vector3> _points = new();

    private LineRenderer _line;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (_points.Contains(transform.position) || _points.Count > 0 &&
            Vector3.Distance(transform.position, _points[^1]) <= 0.05f) return;
        _points.Add(transform.position);
        _line.positionCount = _points.Count;
        _line.SetPositions(_points.ToArray());
    }
}

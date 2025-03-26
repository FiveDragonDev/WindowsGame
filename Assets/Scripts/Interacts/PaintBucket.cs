using UnityEngine;

public class PaintBucket : Pickupable
{
    [SerializeField] private SpriteRenderer _colorPart;
    [SerializeField] private GameObject _paintZonePrefab;

    private Color _color;

    private void Start() => GenerateColor();

    protected override void OnPointerUse()
    {
        var paintRenderer = Instantiate(_paintZonePrefab, transform.position,
            Quaternion.identity).GetComponent<SpriteRenderer>();
        paintRenderer.color = _color;

        GenerateColor();
    }

    private void GenerateColor()
    {
        _color = Random.ColorHSV(0, 1, 0.9f, 1, 0.9f, 1);
        _colorPart.color = _color;
    }
}

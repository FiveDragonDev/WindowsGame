using UnityEngine;

public class File : StandartBulletBase
{
    public override float CheckRadius => 0.25f;

    [SerializeField] private Sprite[] _sprites;

    private SpriteRenderer _renderer;

    private void Awake() => _renderer = GetComponent<SpriteRenderer>();

    public override void Setup(IGun gun, float speed, float damage)
    {
        base.Setup(gun, speed, damage);
        _renderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}

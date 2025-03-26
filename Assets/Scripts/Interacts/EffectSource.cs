using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public abstract class EffectSource : Pickupable
{
    public bool PlayerEntered { get; private set; }
    public ReadOnlyCollection<IEffectable> Entered
    {
        get
        {
            foreach (IEffectable effectable in _entered)
            {
                if (effectable == null) _entered.Remove(effectable);
            }
            return _entered.ToList().AsReadOnly();
        }
    }

    public IEffect Effect => _effect;

    [SerializeField] protected bool _usePlayer;
    [SerializeReference] protected IEffect _effect;

    private readonly HashSet<IEffectable> _entered = new();

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.TryGetComponent(out IEffectable effectable))
            AddEffectable(effectable);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IEffectable effectable))
            RemoveEffectable(effectable);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IEffectable effectable))
            AddEffectable(effectable);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IEffectable effectable))
            RemoveEffectable(effectable);
    }
    private void OnMouseEnter()
    {
        if (_usePlayer) return;
        AddEffectable(PlayerController.Singleton);
        PlayerEntered = true;
    }
    private void OnMouseExit()
    {
        RemoveEffectable(PlayerController.Singleton);
        PlayerEntered = false;
    }

    private void AddEffectable(IEffectable effectable)
    {
        OnEnter(effectable);
        _entered.Add(effectable);
    }
    private void RemoveEffectable(IEffectable effectable)
    {
        OnExit(effectable);
        _entered.Remove(effectable);
    }

    protected virtual void OnEnter(IEffectable effectable) { }
    protected virtual void OnExit(IEffectable effectable) { }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : IEnumerable<T>
{
    public int AllAmount => All.Count;
    public int UsedAmount => _used.Count;
    public int AvaibleAmount => _available.Count;

    protected HashSet<T> All => _available.Concat(_used).ToHashSet();

    protected readonly Queue<T> _available = new();
    protected readonly HashSet<T> _used = new();

    public virtual T GetItem()
    {
        if (_available.Count == 0) CreateNewObject();

        T pooledItem = _available.Dequeue();
        _used.Add(pooledItem);
        return pooledItem;
    }
    public virtual void Release(T item)
    {
        if (!_used.Contains(item) || _used.Remove(item))
            _available.Enqueue(item);
    }
    public virtual void ReleaseAll()
    {
        while (UsedAmount > 0) Release(_used.First());
    }

    protected virtual void CreateNewObject()
    {
        T newItem = System.Activator.CreateInstance<T>();
        _available.Enqueue(newItem);
    }

    public IEnumerator<T> GetEnumerator() => All.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed class GameObjectPool : Pool<GameObject>
{
    public Transform Parent => _parent;
    public GameObject Prefab => _prefab;

    private readonly Transform _parent;
    private readonly GameObject _prefab;

    public GameObjectPool(GameObject prefab, int amount = 0, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
        for (int i = 0; i < amount; i++) CreateNewObject();
    }

    public override GameObject GetItem()
    {
        if (_available.Count == 0) CreateNewObject();

        var pooledItem = _available.Dequeue();
        _used.Add(pooledItem);
        pooledItem.SetActive(true);
        return pooledItem;
    }
    public override void Release(GameObject item)
    {
        if (!_used.Contains(item) || _used.Remove(item))
        {
            item.SetActive(false);
            _available.Enqueue(item);
        }
    }
    public override void ReleaseAll()
    {
        foreach (var item in _used)
        {
            item.SetActive(false);
            _available.Enqueue(item);
        }
        _used.Clear();
    }

    protected override void CreateNewObject()
    {
        var gameObject = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _parent);
        gameObject.SetActive(false);
        _available.Enqueue(gameObject);
    }
}

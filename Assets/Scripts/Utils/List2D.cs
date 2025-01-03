using System.Collections.Generic;

public class List2D<T1, T2>
{
    public int Count => List1.Count;

    public (T1, T2) this[int index]
    {
        get => (List1[index], List2[index]);
        set
        {
            List1[index] = value.Item1;
            List2[index] = value.Item2;
        }
    }

    public List<T1> List1 => _lists.Item1;
    public List<T2> List2 => _lists.Item2;

    private readonly (List<T1>, List<T2>) _lists = new();

    public List2D(int capacity)
    {
        _lists.Item1 = new(capacity);
        _lists.Item2 = new(capacity);
    }
    public List2D()
    {
        _lists.Item1 = new();
        _lists.Item2 = new();
    }

    public int IndexOf(T1 item) => List1.IndexOf(item);
    public int IndexOf(T2 item) => List2.IndexOf(item);
    public void Insert(int index, (T1, T2) item)
    {
        List1.Insert(index, item.Item1);
        List2.Insert(index, item.Item2);
    }
    public void RemoveAt(int index)
    {
        List1.RemoveAt(index);
        List2.RemoveAt(index);
    }
    public void AddRange(IEnumerable<(T1, T2)> collection)
    {
        foreach (var item in collection)
        {
            List1.Add(item.Item1);
            List2.Add(item.Item2);
        }
    }
    public void Add((T1, T2) item)
    {
        List1.Add(item.Item1);
        List2.Add(item.Item2);
    }
    public void Clear()
    {
        List1.Clear();
        List2.Clear();
    }
    public bool Contains(T1 item) => List1.Contains(item);
    public bool Contains(T2 item) => List2.Contains(item);
    public IEnumerator<(T1, T2)> GetEnumerator()
    {
        for (int i = 0; i < Count; i++) yield return (List1[i], List2[i]);
    }
}

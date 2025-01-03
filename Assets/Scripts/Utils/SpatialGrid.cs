using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class SpatialGrid<T>
{
    public ReadOnlyDictionary<Vector2Int, HashSet<T>> Grid => new(_grid);
    public int CellSize => _cellSize;

    private readonly int _cellSize = 1;
    protected readonly Dictionary<Vector2Int, HashSet<T>> _grid = new();

    public SpatialGrid(int cellSize) => _cellSize = Mathf.Max(cellSize, 1);

    public void Clear() => _grid.Clear();

    public virtual void AddItem(T item, Vector2 position)
    {
        Vector2Int cellPosition = GetCellIndex(position);
        if (!_grid.ContainsKey(cellPosition))
            _grid[cellPosition] = new();
        _grid[cellPosition].Add(item);
    }
    public virtual List<T> GetNearbyItems(Vector2 position)
    {
        List<T> nearbyPoints = new();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighborCell = GetCellIndex(new(position.x + x, position.y + y));
                if (_grid.ContainsKey(neighborCell)) nearbyPoints.AddRange(_grid[neighborCell]);
            }
        }

        return nearbyPoints;
    }

    protected Vector2Int GetCellIndex(Vector2 position) =>
        new(Mathf.FloorToInt(position.x / CellSize),
            Mathf.FloorToInt(position.y / CellSize));
}

public class Tile
{
    private CustomGrid<Tile> _grid;

    private int _x;
    private int _y;
    private bool _hasBeenVisited;
    public bool IsWalkable { get; private set; }
    public bool IsVisited { get => _hasBeenVisited; }

    public int X => _x;
    public int Y => _y;

    public Tile(CustomGrid<Tile> grid, int x, int y, bool walkable = true)
    {
        _grid = grid;
        _x = x;
        _y = y;
        _hasBeenVisited = false;
        IsWalkable = walkable;
    }

    public void VisitTile(bool triggerChange = true)
    {
        _hasBeenVisited = true;
        if(triggerChange)
            _grid.TriggerGridChanges(_x, _y);
        GameManager.Instance.RemainingSquares--;
    }

    public bool SetWalkable(bool isWalkable, bool triggerChange = true)
    {
        IsWalkable = isWalkable && !_hasBeenVisited;
        if (triggerChange)
            _grid.TriggerGridChanges(_x, _y);
        return IsWalkable;
    }

    public void Restart()
    {
        _hasBeenVisited = false;
        IsWalkable = false;
    }

    public override string ToString()
    {
        return _hasBeenVisited.ToString();
    }
}

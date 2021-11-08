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

    public void VisitTile()
    {
        _hasBeenVisited = true;
        _grid.TriggerGridChanges(_x, _y);
        GameManager.Instance.RemainingSquares--;
    }

    public bool SetWalkable(bool isWalkable)
    {
        IsWalkable = isWalkable && !_hasBeenVisited;
        _grid.TriggerGridChanges(_x, _y);
        return IsWalkable;
    }

    public void Restart()
    {
        _hasBeenVisited = false;
        IsWalkable = false;
        _grid.TriggerGridChanges(_x, _y);
    }

    public override string ToString()
    {
        return _hasBeenVisited.ToString();
    }
}

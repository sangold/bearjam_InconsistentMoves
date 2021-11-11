using DG.Tweening;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;

    public static GridManager Instance
    {
        get { return _instance; }
    }

    private CustomGrid<Tile> _grid;
    private VisualTile[,] _visualGrid;
    private Tile _activeTile;
    [SerializeField] private VisualTile[] _tilePrefabList;
    private int _numOfMoves;

    public bool IsThereLegalMove => _numOfMoves > 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        _grid = new CustomGrid<Tile>(GameManager.MAP_WIDTH, GameManager.MAP_HEIGHT, 1f, Vector3.zero, (CustomGrid<Tile> g, int x, int y) => new Tile(g, x, y));
        _visualGrid = new VisualTile[GameManager.MAP_WIDTH, GameManager.MAP_HEIGHT];
        for(int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                VisualTile visualTile = Instantiate(_tilePrefabList[0]);
                visualTile.Init(x, y, transform);
                visualTile.transform.position = _grid.GetWorldPosition(x, y);
                _visualGrid[x, y] = visualTile;
            }
        }

        _grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    public Tile HoverTile(Vector3 mousePos)
    {
        Tile hoveredTile = _grid.GetGridObject(mousePos);

        if (_activeTile != null && _activeTile != hoveredTile)
        {
            _visualGrid[_activeTile.X, _activeTile.Y]?.SetHighlight(false);
        }

        if (hoveredTile == null)
        {
            _activeTile = null;
            return _activeTile;
        }

        if (hoveredTile != _activeTile)
        {
            _visualGrid[hoveredTile.X, hoveredTile.Y]?.SetHighlight(true);
            _activeTile = hoveredTile;
        }

        return _activeTile;
    }

    public void CalculateNewMoves()
    {
        ResetWalkable();

        PlayerType pt = GameManager.Instance.Player.CurrentType;

        foreach (Vector2Int move in pt.Moves)
        {
            if (!pt.isInfinite)
            {
                Tile tile = _grid.GetGridObject(GameManager.Instance.Player.CurrentTile.X + move.x, GameManager.Instance.Player.CurrentTile.Y + move.y);
                if (tile == null) continue;
                if(tile.SetWalkable(true)) _numOfMoves++;
            }
            else
            {
                int iteration = 1;
                while (iteration < Mathf.Max(GameManager.MAP_WIDTH, GameManager.MAP_HEIGHT))
                {
                    Tile tile = _grid.GetGridObject(GameManager.Instance.Player.CurrentTile.X + move.x * iteration, GameManager.Instance.Player.CurrentTile.Y + move.y * iteration);
                    if (tile == null)
                        break;
                    if(tile.SetWalkable(true)) _numOfMoves++;
                    iteration++;
                }
            }
        }
    }

    private void ResetWalkable()
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                _grid.GetGridObject(x, y).SetWalkable(false);
            }
        }

        _numOfMoves = 0;
    }

    public void ResetAll()
    {
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                _visualGrid[x, y].Reset();
                _grid.GetGridObject(x, y).Restart();
            }
        }

        _numOfMoves = 0;
    }

    public CustomGrid<Tile> GetGrid()
    {
        return _grid;
    }

    private void Grid_OnGridValueChanged(object sender, CustomGrid<Tile>.OnGridValueChangedEventArgs e)
    {
        if (e.Tile == null) return;

        VisualTile visualTile = _visualGrid[e.x, e.y];
        visualTile.SetBgColor(e.Tile.IsVisited);
        visualTile.SetWalkable(e.Tile.IsWalkable, GameManager.Instance.CurrentTile);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    private CustomGrid<Tile> _grid;
    private PlayerType _currentType;
    private PlayerType[] _types;
    private Tile _currentTile;
    private SpriteRenderer _spriteRenderer;

    public PlayerType CurrentType { get => _currentType; }
    public Tile CurrentTile { get => _currentTile; }

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _types = Resources.LoadAll<PlayerType>("PlayerTypes");
        _grid = FindObjectOfType<GridManager>().GetGrid();
        SetRandomType();
        MoveTo(0, 0);
    }

    private void SetType(PlayerType type)
    {
        if (type == _currentType) return;
        _currentType = type;
        _spriteRenderer.sprite = _currentType.Sprite;
    }

    public void SetRandomType()
    {
        SetType(_types[Random.Range(0, _types.Length)]);
    }

    public bool MoveTo(int x, int y)
    {
        Tile target = _grid.GetGridObject(x, y);
        
        if (target == null) return false;
        

        if (target.IsWalkable && _currentTile != target)
        {
            _currentTile = target;
            transform.position = _grid.GetWorldPosition(x, y);
            return true;
        }

        return false;
    }
}
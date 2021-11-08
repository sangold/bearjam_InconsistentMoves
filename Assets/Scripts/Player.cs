using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    private PlayerType _currentType;
    private Tile _currentTile;
    private SpriteRenderer _spriteRenderer;

    public PlayerType CurrentType { get => _currentType; }
    public Tile CurrentTile { get => _currentTile; }

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        MoveTo(0, 0);
    }

    public void SetType(PlayerType type)
    {
        if (type == _currentType) return;
        _currentType = type;
        _spriteRenderer.sprite = _currentType.Sprite;
    }

    public bool MoveTo(int x, int y)
    {
        
        Tile target = GridManager.Instance.GetGrid().GetGridObject(x, y);
        
        if (target == null) return false;
        

        if (target.IsWalkable && _currentTile != target)
        {
            _currentTile = target;
            Vector3 pos = GridManager.Instance.GetGrid().GetWorldPosition(x, y);
            pos.z = -5f;
            transform.position = pos;
            return true;
        }

        return false;
    }

    public void TeleportTo(int x, int y)
    {
        Tile target = GridManager.Instance.GetGrid().GetGridObject(x, y);
        if (target == null) return;

        _currentTile = target;
        Vector3 pos = GridManager.Instance.GetGrid().GetWorldPosition(x, y);
        pos.z = -5f;
        transform.position = pos;
    }
}

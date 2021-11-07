using IC.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public const int MAP_WIDTH = 10, MAP_HEIGHT = 10;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Player _player;

    public Player Player { get => _player; }

    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    private void Update()
    {
        Vector3 mousePos = Utils.GetMouseWorldPosition();

        Tile hoveredTile = _gridManager.HoverTile(mousePos);

        if (hoveredTile == null) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if(_player.MoveTo(hoveredTile.X, hoveredTile.Y))
            {
                hoveredTile.VisitTile();
                _player.SetRandomType();
                _gridManager.CalculateNewMoves();
                if (!_gridManager.IsThereLegalMove)
                    Debug.Log("GAME OVER");
            }
        }
    }
}

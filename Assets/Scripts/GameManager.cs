using IC.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public const int MAP_WIDTH = 8, MAP_HEIGHT = 8;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Player _player;
    private PlayerType[] _types;
    private PlayerType[] _nextTypes;

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

        _types = Resources.LoadAll<PlayerType>("PlayerTypes");
        _nextTypes = new PlayerType[2];

    }

    private void Start()
    {
        Camera.main.transform.position = new Vector3(MAP_WIDTH / 2f, MAP_HEIGHT / 2f, -10f);
        SetRandomPlayerType(false);
        _player.MoveTo(Mathf.FloorToInt(MAP_WIDTH / 2), Mathf.FloorToInt(MAP_HEIGHT / 2));
        _gridManager.GetGrid().GetGridObject(Mathf.FloorToInt(MAP_WIDTH / 2), Mathf.FloorToInt(MAP_HEIGHT / 2)).VisitTile();
        _gridManager.CalculateNewMoves();
    }

    private void SetRandomPlayerType(bool fromNext)
    {
        if(!fromNext)
            _player.SetType(_types[Random.Range(0, _types.Length)]);
        else
            _player.SetType(_nextTypes[Random.Range(0, _nextTypes.Length)]);
        SetNextRandomPlayerType();
    }

    private void SetNextRandomPlayerType()
    {
        _nextTypes[0] = _types[Random.Range(0, _types.Length)];
        _nextTypes[1] = _types[Random.Range(0, _types.Length)];
        UIManager.Instance.SetNextPieces(_nextTypes[0].Sprite, _nextTypes[1].Sprite);
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
                SetRandomPlayerType(true);
                _gridManager.CalculateNewMoves();
                if (!_gridManager.IsThereLegalMove)
                    Debug.Log("GAME OVER");
            }
        }
    }
}

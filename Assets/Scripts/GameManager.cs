using IC.Utils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public const int MAP_WIDTH = 8, MAP_HEIGHT = 8;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Player _player;
    private PlayerType[] _types;
    private PlayerType[] _nextTypes;
    private float _elapsedTime;
    private int _remainingSquares;

    public int RemainingSquares { 
        get => _remainingSquares; 
        set {
            _remainingSquares = value;
            UIManager.Instance.UpdateRemainingSquares(_remainingSquares);
        } 
    }

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

        _types = Resources.LoadAll<PlayerType>("PlayerTypes");
        _nextTypes = new PlayerType[2];

    }

    private void Start()
    {
        Camera.main.transform.position = new Vector3(MAP_WIDTH / 2f, MAP_HEIGHT / 2f + 1f, -10f);
        Init();
    }

    private void Init()
    {
        _elapsedTime = 0;
        RemainingSquares = MAP_HEIGHT * MAP_WIDTH;
        SetRandomPlayerType(false);
        _player.TeleportTo(Mathf.FloorToInt(MAP_WIDTH / 2), Mathf.FloorToInt(MAP_HEIGHT / 2));
        _gridManager.GetGrid().GetGridObject(Mathf.FloorToInt(MAP_WIDTH / 2), Mathf.FloorToInt(MAP_HEIGHT / 2)).VisitTile();
        _gridManager.CalculateNewMoves();
    }

    public void Restart()
    {
        _gridManager.ResetAll();
        Init();
    }

    private void SetRandomPlayerType(bool fromNext)
    {
        if (!fromNext)
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
        _elapsedTime += Time.deltaTime;
        UIManager.Instance.UpdateTimer(_elapsedTime);

        Vector3 mousePos = Utils.GetMouseWorldPosition();

        Tile hoveredTile = _gridManager.HoverTile(mousePos);

        if (hoveredTile == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_player.MoveTo(hoveredTile.X, hoveredTile.Y))
            {
                hoveredTile.VisitTile();
                SetRandomPlayerType(true);
                if (RemainingSquares == 0)
                    UIManager.Instance.OpenPopup(VictoryPopup.PopupType.VICTORY, _elapsedTime);
                _gridManager.CalculateNewMoves();
                if (!_gridManager.IsThereLegalMove)
                    UIManager.Instance.OpenPopup(VictoryPopup.PopupType.DEFEAT, RemainingSquares);

            }
        }

    }
}

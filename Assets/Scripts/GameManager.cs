using Chesslitaire.Utils;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        WAITING,
        MOVING,
        CALCULATING,
        VICTORY,
        DEFEAT
    }
    private static GameManager _instance;
    public const int MAP_WIDTH = 8, MAP_HEIGHT = 8;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Player _player;
    [SerializeField] private AudioClip _startSound;
    private PlayerType[] _types;
    private PlayerType[] _nextTypes;
    private float _elapsedTime;
    private int _remainingSquares;
    private State _currentState;

    public int RemainingSquares { 
        get => _remainingSquares; 
        set {
            _remainingSquares = value;
            UIManager.Instance.UpdateRemainingSquares(_remainingSquares);
        } 
    }

    public Player Player { get => _player; }
    public Tile CurrentTile { get => Player.CurrentTile; }

    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        DOTween.Init();
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
        _player.OnReadyToChange += ChangePlayerType;
        Init();
    }

    private void ChangePlayerType(object sender, System.EventArgs e)
    {
        SetRandomPlayerType(true);
        _gridManager.CalculateNewMoves();
        if (!_gridManager.IsThereLegalMove)
            Defeat();
    }

    private void Init()
    {
        _currentState = State.MOVING;
        _elapsedTime = 0;
        RemainingSquares = MAP_HEIGHT * MAP_WIDTH;
        SetRandomPlayerType(false);
        int x = Random.Range(0, MAP_WIDTH);
        int y = Random.Range(0, MAP_HEIGHT);
        _player.TeleportTo(x, y);
        _gridManager.GetGrid().GetGridObject(x,y).VisitTile();
        _currentState = State.CALCULATING;
        _gridManager.CalculateNewMoves();
        _currentState = State.WAITING;
        SoundManager.Instance.PlaySound(_startSound);
    }

    public void Restart()
    {
        _gridManager.ResetAll();
        Init();
    }

    public void SetState(State state)
    {
        if (_currentState != state)
            _currentState = state;
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

    private void Victory()
    {
        SetState(State.VICTORY);
        UIManager.Instance.OpenPopup(UIPopup.PopupType.VICTORY, _elapsedTime);
    }

    private void Defeat()
    {
        SetState(State.DEFEAT);
        UIManager.Instance.OpenPopup(UIPopup.PopupType.DEFEAT, RemainingSquares);
    }

    private void Update()
    {
        if (_currentState == State.VICTORY)
            return;
        if (_currentState == State.DEFEAT)
            return;

        _elapsedTime += Time.deltaTime;
        UIManager.Instance.UpdateTimer(_elapsedTime);

        Vector3 mousePos = Utils.GetMouseWorldPosition();

        Tile hoveredTile = _gridManager.HoverTile(mousePos);

        if (hoveredTile == null) return;

        if(_currentState != State.WAITING) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            if (_player.MoveTo(hoveredTile.X, hoveredTile.Y))
            {
                hoveredTile.VisitTile();
                if (RemainingSquares == 0)
                    Victory();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Victory();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            Defeat();
        }

    }
}

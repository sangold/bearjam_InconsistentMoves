using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{
    private PlayerType _currentType;
    private Tile _currentTile;
    private SpriteRenderer _spriteRenderer;

    public EventHandler OnReadyToChange;

    public PlayerType CurrentType { get => _currentType; }
    public Tile CurrentTile { get => _currentTile; }

    public float MoveDuration { get; private set; }
    [SerializeField] private AudioClip _wooshSound;
    [SerializeField] private AudioClip _popSound;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
            GameManager.Instance.SetState(GameManager.State.MOVING);
            Vector3 pos = GridManager.Instance.GetGrid().GetWorldPosition(x, y);
            pos.x += .5f;
            pos.y += .5f;
            pos.z = -5f;
            MoveDuration = .2f + .03f * (transform.position - pos).magnitude;
            Sequence moveSequence = DOTween.Sequence();
            moveSequence
                .Append(transform.DOMove(pos, MoveDuration).SetEase(Ease.OutQuad).OnStart(() => SoundManager.Instance.PlaySound(_wooshSound, 1f, true)))
                .Append(transform.DOScale(new Vector3(0, 0, 0), .15f).OnComplete(() => {
                    SoundManager.Instance.PlaySound(_popSound, 1f, true);
                    OnReadyToChange?.Invoke(this, null);
                }))
                .Append(transform.DOScale(new Vector3(1, 1, 1), .3f))
                //.Append(transform.DOPunchScale(new Vector3(-.1f, -.1f, -.1f), .35f, 2, 0f).SetEase(Ease.OutCubic))
                .OnComplete(() => GameManager.Instance.SetState(GameManager.State.WAITING));
            _currentTile = target;
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
        pos.x += .5f;
        pos.y += .5f;
        pos.z = -5f;
        transform.position = pos;
    }
}

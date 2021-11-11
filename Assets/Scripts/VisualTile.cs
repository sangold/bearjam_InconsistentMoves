using DG.Tweening;
using UnityEngine;

public class VisualTile : MonoBehaviour
{
    private int _x, _y, _animationID, _textureAnimID;
    [SerializeField] private MeshRenderer _mr;
    [SerializeField] private GameObject _highlightGO;
    private Material _backFaceMaterial;
    private Color _walkableColor = new Color(r: 0.525f, g: 0.908f, b: 0.564f);
    private Color _visitedColor = new Color(r: 1f, g: 0.8128f, b: 0.243f);
    private bool _isVisited;
    private bool _isFlipped;
    private Quaternion unflippedRot = Quaternion.identity * Quaternion.Euler(180, 0, 0);
    private Quaternion flippedRot = Quaternion.identity * Quaternion.Euler(0, 0, 0);

    private void Awake()
    {
        _backFaceMaterial = _mr.materials[2];
    }
    public void Init(int x, int y, Transform parent)
    {
        _x = x;
        _y = y;
        _animationID = x + 10 * y;
        _textureAnimID = 1000+ x * 10 + y * 100;
        if (_x % 2 == 0 && _y % 2 == 0 || _x % 2 != 0 && _y % 2 != 0)
            _mr.materials[0].SetColor("_Color", new Color(.153f, .204f, .412f));
        else
            _mr.materials[0].SetColor("_Color", new Color(.118f, .153f, .286f));

        SetBgColor(false, 0);
        SetHighlight(false);
        transform.SetParent(parent);
    }

    public void SetBgColor(bool isVisited, float moveDuration)
    {
        if (isVisited == _isVisited) return;
        if (isVisited)
        {
            _backFaceMaterial
                .DOFloat(.25f, "_TransitionProgression", .4f)
                .SetDelay(moveDuration + .1f)
                .SetId(_textureAnimID)
                .SetEase(Ease.OutExpo);
            if(!_isFlipped)
            {
                KillRotation();
                StartRotation(new Vector3(180, 0, 0));
                _isFlipped = true;
            }
        }
        else
        {
            if (_isFlipped)
            {
                KillRotation();
                StartRotation(new Vector3(-180, 0, 0));
                _isFlipped = false;
            }

        }

        _isVisited = isVisited;
    }

    private void StartRotation(Vector3 rotationAngle, float delay = 0f)
    {
        _mr.transform
            .DOBlendableLocalRotateBy(rotationAngle, .5f, RotateMode.LocalAxisAdd)
            .SetDelay(delay)
            .SetId(_animationID);
    }

    private void KillRotation()
    {
        DOTween.Kill(_animationID);
        _mr.transform.rotation = _isFlipped ? flippedRot : unflippedRot;
    }

    public void Reset()
    {
        _highlightGO.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        _isFlipped = false;
        _isVisited = false;
        SetHighlight(false);
        KillRotation();
        DOTween.Kill(_textureAnimID);
        _backFaceMaterial.SetFloat("_TransitionProgression", 0f);
    }

    public void SetHighlight(bool isActive)
    {
        _highlightGO.SetActive(isActive);
    }

    public void SetWalkable(bool isWalkable, Tile currentTile)
    {
        float distX = currentTile.X - _x;
        float distY = currentTile.Y - _y;

        float dist = Mathf.Max(Mathf.Abs(distX), Mathf.Abs(distY));

        Vector3 dir;
        if (Mathf.Abs(distX) > Mathf.Abs(distY))
            dir = new Vector3(0, 1,0) * Mathf.Sign(distX);
        else
            dir = new Vector3(1, 0, 0) * Mathf.Sign(distY);


        KillRotation();

        if (isWalkable)
        {
            _backFaceMaterial.SetFloat("_TransitionProgression", 0f);
            if (!_isFlipped)
            {
                StartRotation(180f * dir, .05f * dist);
                _isFlipped = true;
            }
            _highlightGO.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            if(_isFlipped && !_isVisited)
            {
                StartRotation(new Vector3(-180, 0, 0));
                _isFlipped = false;
            }
            _highlightGO.GetComponent<SpriteRenderer>().color = new Color(1f, 0.36f, 0.36f, .21f);
        }
    }
}

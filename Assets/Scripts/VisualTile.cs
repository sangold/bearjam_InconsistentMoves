using System.Collections;
using UnityEngine;

public class VisualTile : MonoBehaviour
{
    private int _x, _y;
    [SerializeField] private MeshRenderer _mr;
    [SerializeField] private GameObject _highlightGO;
    private Color _color;
    private bool _isMoving = false;
    private bool _isVisited;
    private bool _isFlipped;
    private Quaternion unflippedRot = Quaternion.identity * Quaternion.Euler(180, 0, 0);
    private Quaternion flippedRot = Quaternion.identity * Quaternion.Euler(0, 0, 0);
    [SerializeField] private float _rotationSpeed = 6f;

    public void Init(int x, int y, Transform parent)
    {
        _x = x;
        _y = y;

        if (_x % 2 == 0 && _y % 2 == 0 || _x % 2 != 0 && _y % 2 != 0)
            _color = new Color(.153f, .204f, .412f);
        else
            _color = new Color(.118f, .153f, .286f);

        SetBgColor(false);
        SetHighlight(false);
        transform.SetParent(parent);

    }

    public void SetBgColor(bool isVisited)
    {
        if (isVisited == _isVisited) return;
        if (isVisited)
        {
            if(!_isFlipped)
            {
                if (_isMoving)
                {
                    StopAllCoroutines();
                    _mr.transform.rotation = unflippedRot;
                }
                StartCoroutine(Rotation(180));
                _isFlipped = true;
            }
        }
        else
        {
            _mr.materials[0].SetColor("_Color", _color);
            if(_isFlipped)
            {
                if (_isMoving)
                {
                    StopAllCoroutines();
                    _mr.transform.rotation = flippedRot;
                }

                StartCoroutine(Rotation(-180));
                _isFlipped = false;
            }
        }

        _isVisited = isVisited;
    }

    public void Reset()
    {
        StopAllCoroutines();
        _highlightGO.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        _mr.materials[0].SetColor("_Color", _color);
        _mr.transform.rotation = Quaternion.identity * Quaternion.Euler(180, 0, 0);
        _isMoving = false;
        _isFlipped = false;
        _isVisited = false;
        SetHighlight(false);
    }

    IEnumerator Rotation(float angle)
    {
        _isMoving = true;

        for(int i = 0; i < Mathf.Abs(angle / _rotationSpeed); i++)
        {
            _mr.transform.Rotate(new Vector3(1, 0, 0) * angle/Mathf.Abs(angle), _rotationSpeed);
            yield return new WaitForSeconds(.01f);
        }

        _isMoving = false;
    }

    public void SetHighlight(bool isActive)
    {
        _highlightGO.SetActive(isActive);
        //if(!_isMoving)
        //{
        //    if(isActive)
        //    {
        //        StartCoroutine(Push(new Vector3(0.5f, 0.5f, .5f)));
        //    } 
        //    else
        //    {
        //        StartCoroutine(Push(new Vector3(0.5f, 0.5f, 0f)));
        //    }
        //}
    }

    public void SetWalkable(bool isWalkable)
    {
        if (isWalkable)
        {
            _highlightGO.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            _mr.materials[0].SetColor("_Color", new Color(r: 0.525f, g: 0.908f, b: 0.564f));
        }
        else
        {
            _mr.materials[0].SetColor("_Color", _color);
            _highlightGO.GetComponent<SpriteRenderer>().color = new Color(1f, 0.36f, 0.36f, .21f);
        }
    }
}

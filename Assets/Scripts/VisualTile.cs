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
    [SerializeField] private float _rotationSpeed = 6f;
    [SerializeField] private float _animDuration = .25f;

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

        if (isVisited && !_isMoving) 
        {
            _highlightGO.GetComponent<SpriteRenderer>().color = new Color(1f, 0.36f, 0.36f, .21f);
            StartCoroutine(Rotation(180));
        }
        else
        {
            _highlightGO.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            _mr.materials[0].SetColor("_Color", _color);
            if(!_isMoving)
                StartCoroutine(Rotation(-180));
        }

        _isVisited = isVisited;
    }

    IEnumerator Rotation(float angle)
    {
        _isMoving = true;

        for(int i = 0; i < Mathf.Abs(angle / _rotationSpeed); i++)
        {
            _mr.transform.Rotate(new Vector3(1, 0, 0), _rotationSpeed);
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
            _mr.materials[0].SetColor("_Color", new Color(r: 0.525f, g: 0.908f, b: 0.564f));
        else
            _mr.materials[0].SetColor("_Color", _color);
    }
}

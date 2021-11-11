using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class VisualTile : MonoBehaviour
{
    private enum DOAnimation
    {
        COMPLETE,
        WALKABLE
    }
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
                KillRotation();
                StartRotation(new Vector3(180, 0, 0));
                _isFlipped = true;
            }
        }
        else
        {
            _mr.materials[0].SetColor("_Color", _color);
            if (_isFlipped)
            {
                KillRotation();
                StartRotation(new Vector3(-180, 0, 0));
                _isFlipped = false;
            }

        }

        _isVisited = isVisited;
    }

    private void StartRotation(Vector3 rotationAngle)
    {
        Debug.Log(rotationAngle);
        _mr.transform
            .DOBlendableLocalRotateBy(rotationAngle, 5f, RotateMode.LocalAxisAdd)
            .SetId(DOAnimation.COMPLETE + _x * 10 + _y *100)
            .OnComplete(()=>Debug.Log("complete"));
    }

    private void KillRotation()
    {
        DOTween.Kill(DOAnimation.COMPLETE + _x * 10 + _y * 100);
        DOTween.Kill(DOAnimation.WALKABLE + _x * 10 + _y * 100);
        _mr.transform.rotation = _isFlipped ? flippedRot : unflippedRot;
    }

    public void Reset()
    {
        _highlightGO.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        _mr.materials[0].SetColor("_Color", _color);
        _isMoving = false;
        _isFlipped = false;
        _isVisited = false;
        KillRotation();
        SetHighlight(false);
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

            _mr.transform
                .DOBlendableLocalRotateBy(360f * dir, .5f, RotateMode.LocalAxisAdd)
                .SetDelay(.02f * dist)
                .SetId(DOAnimation.WALKABLE + _x * 10 + _y * 100);

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

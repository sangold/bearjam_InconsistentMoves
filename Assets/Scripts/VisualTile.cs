using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTile : MonoBehaviour
{
    private int _x, _y;
    [SerializeField] private SpriteRenderer _tileBg;
    [SerializeField] private GameObject _tileHighlight;
    [SerializeField] private GameObject _tileWalkable;

    public void SetBgColor(bool isVisited)
    {
        if (isVisited)
            _tileBg.color = Color.green;
        else if (_x % 2 == 0 && _y % 2 == 0 || _x % 2 != 0 && _y % 2 != 0)
            _tileBg.color = Color.black;
        else
            _tileBg.color = Color.white;
    }

    public void SetHighlight(bool isActive)
    {
        _tileHighlight.SetActive(isActive);
    }

    public void Init(int x, int y, Transform parent)
    {
        _x = x;
        _y = y;
        SetBgColor(false);
        SetHighlight(false);
        transform.SetParent(parent);
    }

    public void SetWalkable(bool isWalkable)
    {
        _tileWalkable.SetActive(isWalkable);
    }
}

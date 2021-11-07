using IC.Utils;
using System;
using UnityEngine;

public class CustomGrid<T>
{
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public T Tile;
        public int x;
        public int y;
    }

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    private T[,] _gridArray;


    public CustomGrid (int width, int height, float cellSize, Vector3 op, Func<CustomGrid<T>, int, int, T> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = op;
        _gridArray = new T[width, height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                _gridArray[x,y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = false;
        if(showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];
            for(int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for(int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = Utils.CreateText(null, _gridArray[x, y]?.ToString(), GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 5000);

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = _gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    public int GetWidth() => _width;
    public int GetHeight() => _height;
    public float GetCellSize() => _cellSize;

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    public void Init()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                continue;
            }
        }
    }

    public void SetGridObject(int x, int y, T value)
    {
        if (isInGridRange(x,y))
        {
            _gridArray[x, y] = value;
            TriggerGridChanges(x, y);
        }
    }

    public void TriggerGridChanges(int x, int y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { Tile = GetGridObject(x,y) ,x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, T value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }


    public T GetGridObject(int x, int y)
    {
        if (isInGridRange(x, y))
            return _gridArray[x, y];
        else
            return default(T);
    }

    public T GetGridObject(Vector3 worldPositon)
    {
        int x, y;
        GetXY(worldPositon, out x, out y);
        return GetGridObject(x, y);
    }

    private bool isInGridRange(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _width && y < _height;
    }

    
}
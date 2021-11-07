using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _nextPiece1;
    [SerializeField] private Image _nextPiece2;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _remainingMoves;

    private static UIManager _instance;

    public static UIManager Instance
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
    }

    public void SetNextPieces(Sprite sp1, Sprite sp2)
    {
        _nextPiece1.sprite = sp1;
        _nextPiece2.sprite = sp2;
    }

    public void UpdateTimer(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        _timerText.text = niceTime;
    }

    public void UpdateRemainingSquares(int number)
    {
        _remainingMoves.text = "Remaining squares: " + number.ToString();
    }
}

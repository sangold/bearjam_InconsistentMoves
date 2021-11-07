using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _nextPiece1;
    [SerializeField] private Image _nextPiece2;

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
}

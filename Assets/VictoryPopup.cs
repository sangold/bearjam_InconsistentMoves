using TMPro;
using UnityEngine;

public class VictoryPopup : MonoBehaviour
{
    public enum PopupType
    {
        VICTORY,
        DEFEAT
    }
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _flavour;

    public void SetOverlay(bool isActive, PopupType popupType = PopupType.VICTORY, float timer = 0f)
    {
        if(isActive)
        {
            if(popupType == PopupType.VICTORY)
            {
                _title.text = "Victory";
                _flavour.text = "You cleared the board in ";
                _timer.text = UIManager.TimerToTime(timer);
            }
            else
            {
                _title.text = "Defeat";
                _flavour.text = "You still had to clear ";
                _timer.text = timer.ToString();
            }
        }

        gameObject.SetActive(isActive);
    }
    public void Restart()
    {
        GameManager.Instance.Restart();
        SetOverlay(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

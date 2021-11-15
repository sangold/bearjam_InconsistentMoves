using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    public enum PopupType
    {
        VICTORY,
        DEFEAT
    }
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _flavour;
    [SerializeField] private Transform _popupTransform;

    public void SetOverlay(bool isActive, PopupType popupType = PopupType.VICTORY, float timer = 0f)
    {
        if(isActive)
        {
            gameObject.GetComponent<Image>().DOFade(0f, .5f).From().SetEase(Ease.OutQuint);
            Sequence openSequence = DOTween.Sequence();

            if (popupType == PopupType.VICTORY)
            {
                Vector3 startPos = _popupTransform.position;
                startPos.y -= 150;
                openSequence
                    .Append(_popupTransform.DOMove(startPos, .5f).SetEase(Ease.OutExpo).From())
                    .Join(_popupTransform.DOScale(.25f, .55f).SetEase(Ease.OutExpo).From());
                _title.text = "Victory";
                _flavour.text = "You cleared the board in ";
                _timer.text = UIManager.TimerToTime(timer);
            }
            else
            {
                openSequence
                    .Append(_popupTransform.DOScale(.25f, .55f).SetEase(Ease.OutExpo).From())
                    .Join(_popupTransform.DOShakePosition(1f,new Vector3(50f, 0, 0), vibrato: 15));
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

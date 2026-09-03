using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UISelect : MonoBehaviour, ISelectHandler
{
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private string textToShow;
    public void OnSelect(BaseEventData eventData)
    {
        infoText.SetText(textToShow);
    }
} // ENd
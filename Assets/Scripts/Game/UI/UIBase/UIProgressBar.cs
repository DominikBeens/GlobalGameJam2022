using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIProgressBar : MonoBehaviour {

    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI valueText;

    public void SetValue(float normalizedValue, string displayText = "") {
        fillImage.fillAmount = normalizedValue;
        valueText.text = displayText;
    }
}

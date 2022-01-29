using UnityEngine;
using TMPro;

public class UITextButton : UIButton {

    [Header("Text Button")]
    [SerializeField] private string buttonText;
    [SerializeField] protected TextMeshProUGUI text;

    public string Text => text.text;

    public void SetText(string s) {
        text.text = s;
    }
}

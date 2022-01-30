using UnityEngine;
using TMPro;

public class UITextButton : UIButton {

    [Header("Text Button")]
    [SerializeField] protected TextMeshProUGUI text;

    public string Text => text.text;

    public void SetText(string s) {
        text.text = s;
    }
}

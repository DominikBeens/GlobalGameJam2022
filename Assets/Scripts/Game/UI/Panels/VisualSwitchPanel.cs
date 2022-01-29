using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VisualSwitchPanel : UIPanel {

    [SerializeField] private Image timerFillImage;

    public override void Tick() {
        base.Tick();
        ProcessTimer();
    }

    private void ProcessTimer() {
        timerFillImage.fillAmount = WorldTypeManager.Instance.TimerNormalized;
    }
}

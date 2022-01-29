using UnityEngine;
using TMPro;

public class ScorePanel : UIPanel {

    [Space]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI distanceText;

    private float gameStartTime;

    public override void Show() {
        base.Show();
        GameEvents.OnPlayerDistanceTraveled.AddListener(HandlePlayerDistanceTraveled);
        GameEvents.OnGameStarted.AddListener(HandleGameStarted);
    }

    public override void Hide() {
        base.Hide();
        GameEvents.OnPlayerDistanceTraveled.RemoveListener(HandlePlayerDistanceTraveled);
        GameEvents.OnGameStarted.RemoveListener(HandleGameStarted);
    }

    public override void Tick() {
        base.Tick();
        ProcessTimeText();
    }

    private void HandlePlayerDistanceTraveled(float total, float frame) {
        distanceText.text = $"Distance: {total:F0}";
    }

    private void HandleGameStarted() {
        gameStartTime = Time.time;
    }

    private void ProcessTimeText() {
        float time = Time.time - gameStartTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        timeText.text = $"Time: {minutes:00}:{seconds:00}";
    }
}

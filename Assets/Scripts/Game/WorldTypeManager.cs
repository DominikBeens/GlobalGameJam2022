using UnityEngine;

public class WorldTypeManager : Manager<WorldTypeManager> {

    [SerializeField] private float interval = 15f;

    public VisualType VisualType { get; private set; }
    public float Timer { get; private set; }
    public float TimerNormalized { get; private set; }

    private bool state;

    public override void Initialize() {
        base.Initialize();
        VisualType = VisualType.Light;
        Game.OnGameLoadingStarted.AddListener(HandleLoadingStarted);
    }

    public override void Deinitialize() {
        base.Deinitialize();
        Game.OnGameLoadingStarted.RemoveListener(HandleLoadingStarted);
    }

    public void StartClock() {
        state = true;
    }

    public void StopClock() {
        state = false;
    }

    public override void Tick() {
        base.Tick();
        if (!state) { return; }

        Timer += Time.deltaTime;
        TimerNormalized = Timer / interval;

        if (Timer < interval) { return; }

        Timer = 0f;
        if (VisualType == VisualType.Light) {
            VisualType = VisualType.Dark;
        } else if (VisualType == VisualType.Dark) {
            VisualType = VisualType.Light;
        }

        GameEvents.OnVisualTypeChanged.Invoke(VisualType);
    }

    private void HandleLoadingStarted() {
        VisualType = VisualType.Light;
        Timer = 0f;
        TimerNormalized = 0f;
    }
}

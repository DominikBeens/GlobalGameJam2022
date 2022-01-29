using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTypeManager : Manager<WorldTypeManager> {

    [SerializeField] private float interval = 15f;

    public VisualType VisualType { get; private set; }
    public float Timer { get; private set; }

    private bool state;

    public override void Initialize() {
        base.Initialize();
        VisualType = VisualType.Light;
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
        if (Timer < interval) { return; }

        Timer = 0f;
        if (VisualType == VisualType.Light) {
            VisualType = VisualType.Dark;
        } else if (VisualType == VisualType.Dark) {
            VisualType = VisualType.Light;
        }

        GameEvents.OnVisualTypeChanged.Invoke(VisualType);
    }
}

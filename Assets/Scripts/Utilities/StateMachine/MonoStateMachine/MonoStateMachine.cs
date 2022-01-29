using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonoStateMachine : MonoState {

    [SerializeField] private Transform stateHolder;
    [SerializeField] private bool autoTick = true;

    public virtual MonoState CurrentState { get; private set; }
    public bool IsInitialized { get; private set; }

    private List<MonoState> states = new List<MonoState>();

    protected virtual void Awake() {
        InitializeState(this);

        Transform root = stateHolder != null ? stateHolder : transform;
        foreach (Transform child in root) {
            MonoState state = child.GetComponent<MonoState>();
            if (!state) { continue; }
            InitializeState(state);
        }

        if (states.Count == 0) {
            Debug.LogWarning("No states found in statemachine!");
        }

        void InitializeState(MonoState state) {
            state.gameObject.SetActive(true);
            state.Initialize(this);
            states.Add(state);
        }

        IsInitialized = true;
    }

    public override void Deinitialize() {
        base.Deinitialize();

        if (!IsInitialized) { return; }
        IsInitialized = false;

        ExitCurrentState();

        foreach (MonoState state in states) {
            state.Deinitialize();
        }
    }

    private void Update() {
        if (autoTick) {
            TickStateMachine();
        }
    }

    public override void Enter(params object[] data) { }
    public override void Exit() { }
    public override void Tick() { }

    public void TickStateMachine() {
        Tick();
        CurrentState?.Tick();
    }

    public void EnterState<T>(params object[] data) where T : MonoState {
        MonoState state = GetState<T>();
        if (state == null) {
            Debug.LogError($"Error entering MonoState: no state of type {typeof(T)} found!");
            return;
        }
        EnterState(state, data);
    }

    // TEMP
    public void EnterState(string name, params object[] data) {
        MonoState state = GetStateByName(name);
        if (state == null) {
            Debug.LogError($"Error entering MonoState: no state with name {name} found!");
            return;
        }
        EnterState(state, data);
    }

    public T GetState<T>() where T : MonoState {
        return (T)states?.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    // TEMP
    public MonoState GetStateByName(string name) {
        return states?.FirstOrDefault(x => x.GetType().ToString() == name);
    }

    public void ExitCurrentState() {
        CurrentState?.Exit();
        CurrentState = null;
    }

    private void EnterState(MonoState state, params object[] data) {
        if (CurrentState == state) { return; }
        ExitCurrentState();
        CurrentState = state;
        CurrentState.Enter(data);
    }
}

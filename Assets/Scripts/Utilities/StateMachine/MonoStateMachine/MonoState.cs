using UnityEngine;

public abstract class MonoState : MonoBehaviour {

    public MonoStateMachine StateMachine { get; private set; }
    public bool IsActive => StateMachine?.CurrentState == this;

    public virtual void Initialize(MonoStateMachine stateMachine) {
        StateMachine = stateMachine;
    }

    public virtual void Deinitialize() { }

    public abstract void Enter(params object[] data);
    public abstract void Exit();
    public abstract void Tick();

    protected bool TryGetValueFromData<T>(object[] data, int index, out T value) {
        value = default;

        if (data == null || data.Length <= index) { return false; }
        object o = data[index];
        if (o.GetType() != typeof(T) && o.GetType().IsSubclassOf(typeof(T)) == false) { return false; }

        value = (T)o;
        return true;
    }
}
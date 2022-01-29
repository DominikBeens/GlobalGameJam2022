using UnityEngine;

interface IManager {
    void Initialize();
    void Deinitialize();
    void Tick();
    void LateTick();
}

public abstract class Manager<T> : Singleton<T>, IManager where T : MonoBehaviour {

    public virtual void Initialize() { }
    public virtual void Deinitialize() { }
    public virtual void Tick() { }
    public virtual void LateTick() { }
}

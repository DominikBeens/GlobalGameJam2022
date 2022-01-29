using System;

public class GameEvent {

    private Action gameEvent;

    public void AddListener(Action action) {
        gameEvent += action;
    }

    public void RemoveListener(Action action) {
        gameEvent -= action;
    }

    public void RemoveAllListeners() {
        gameEvent = null;
    }

    public void Invoke() {
        gameEvent?.Invoke();
    }
}

public class GameEvent<T> {

    private Action<T> gameEvent;

    public void AddListener(Action<T> action) {
        gameEvent += action;
    }

    public void RemoveListener(Action<T> action) {
        gameEvent -= action;
    }

    public void RemoveAllListeners() {
        gameEvent = null;
    }

    public void Invoke(T data) {
        gameEvent?.Invoke(data);
    }
}

public class GameEvent<T, U> {

    private Action<T, U> gameEvent;

    public void AddListener(Action<T, U> action) {
        gameEvent += action;
    }

    public void RemoveListener(Action<T, U> action) {
        gameEvent -= action;
    }

    public void RemoveAllListeners() {
        gameEvent = null;
    }

    public void Invoke(T data, U data2) {
        gameEvent?.Invoke(data, data2);
    }
}

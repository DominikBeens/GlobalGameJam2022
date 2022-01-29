using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class UIButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

    protected bool isHovered;
    protected bool isPressed;

    private Vector2 pointerDownPosition;

    private List<Action> listeners = new List<Action>();
    private List<Action<UIButtonBase>> buttonListeners = new List<Action<UIButtonBase>>();

    public bool IsInteractable { get; protected set; } = true;

    protected virtual bool allowClick => true;

    public void AddListener(Action listener) {
        if (listeners.Contains(listener)) { return; }
        listeners.Add(listener);
    }

    public void AddListener(Action<UIButtonBase> listener) {
        if (buttonListeners.Contains(listener)) { return; }
        buttonListeners.Add(listener);
    }

    public void RemoveListener(Action listener) {
        listeners.Remove(listener);
    }

    public void RemoveListener(Action<UIButtonBase> listener) {
        buttonListeners.Remove(listener);
    }

    public void RemoveAllListeners() {
        for (int i = listeners.Count - 1; i >= 0; i--) {
            RemoveListener(listeners[i]);
        }
        listeners.Clear();

        for (int i = buttonListeners.Count - 1; i >= 0; i--) {
            RemoveListener(buttonListeners[i]);
        }
        buttonListeners.Clear();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHovered = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        pointerDownPosition = eventData.position;
        isPressed = true;
        HandlePointerDown();
    }

    public void OnPointerUp(PointerEventData eventData) {
        bool click = isHovered || eventData.position == pointerDownPosition;
        bool drag = eventData.dragging;

        isPressed = false;

        if (click && !drag) {
            HandleClick();
        } else {
            HandleCancelClick();
        }
    }

    protected virtual void HandlePointerDown() { }

    protected virtual void HandleClick() {
        if (!allowClick) { return; }

        for (int i = listeners.Count - 1; i >= 0; i--) {
            listeners[i]?.Invoke();
        }
        for (int i = buttonListeners.Count - 1; i >= 0; i--) {
            buttonListeners[i]?.Invoke(this);
        }
    }

    protected virtual void HandleCancelClick() { }

    protected void ForceClick() {
        if (isHovered) {
            HandleClick();
        } else {
            HandleCancelClick();
        }
        isHovered = false;
        isPressed = false;
    }
}

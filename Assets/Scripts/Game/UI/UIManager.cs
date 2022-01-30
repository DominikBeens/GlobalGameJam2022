using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

public class UIManager : Manager<UIManager> {

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas blockerCanvas;
    [SerializeField] private int popupSortingOrderStart = 10;

    private UIPanel[] panels;
    private List<UIPanel> activePopups = new List<UIPanel>();

    private Button blockerCanvasButton;
    private int popupSortIndex;

    public bool HasActivePopups => activePopups.Count > 0;

    public override void Initialize() {
        base.Initialize();

        SetupBlockerCanvas();

        popupSortIndex = popupSortingOrderStart;

        panels = mainCanvas.GetComponentsInChildren<UIPanel>(true);
        foreach (UIPanel panel in panels) {
            panel.gameObject.SetActive(true);
            panel.Initialize();
        }

        UIPanel.OnUIPanelShow.AddListener(OnUIPanelShowHandler);
        UIPanel.OnUIPanelHide.AddListener(OnUIPanelHideHandler);

        Game.OnGameLoadingStarted.AddListener(OnGameLoadingStarted);
    }

    public override void Deinitialize() {
        base.Deinitialize();
        foreach (UIPanel panel in panels) {
            panel.Deinitialize();
        }

        UIPanel.OnUIPanelShow.RemoveListener(OnUIPanelShowHandler);
        UIPanel.OnUIPanelHide.RemoveListener(OnUIPanelHideHandler);

        Game.OnGameLoadingStarted.RemoveListener(OnGameLoadingStarted);

        blockerCanvasButton.onClick.RemoveListener(OnBlockerCanvasClicked);
    }

    public override void Tick() {
        base.Tick();
        if (panels == null) { return; }
        for (int i = panels.Length - 1; i >= 0; i--) {
            panels[i].Tick();
        }
    }

    public override void LateTick() {
        base.LateTick();
        if (panels == null) { return; }
        for (int i = panels.Length - 1; i >= 0; i--) {
            panels[i].LateTick();
        }
    }

    public void HandleBackButtonPressed() {
        if (!HasActivePopups) { return; }
        int lastIndex = activePopups.Count - 1;
        UIPanel panel = activePopups[lastIndex];
        activePopups.RemoveAt(lastIndex);
        panel.HideThroughBackButton();
    }

    private void SetupBlockerCanvas() {
        blockerCanvas.gameObject.SetActive(true);
        blockerCanvas.enabled = false;

        blockerCanvasButton = blockerCanvas.GetComponent<Button>();
        blockerCanvasButton.onClick.AddListener(OnBlockerCanvasClicked);
    }

    private void OnUIPanelShowHandler(UIPanel panel) {
        if (panel.IsBackButtonClosable) {
            activePopups.Add(panel);
            panel.SetSortingOrder(popupSortIndex++);
        }
        UpdateBlocker();
    }

    private void OnUIPanelHideHandler(UIPanel panel) {
        if (panel.IsBackButtonClosable) {
            activePopups.Remove(panel);
            panel.ResetSortingOrder();
        }
        if (!HasActivePopups) {
            popupSortIndex = popupSortingOrderStart;
        }
        UpdateBlocker();
    }

    private void OnBlockerCanvasClicked() {
        if (!HasActivePopups) { return; }

        int lastIndex = activePopups.Count - 1;
        UIPanel panel = activePopups[lastIndex];
        if (!panel.BackgroundBlockerClosesPanel) { return; }

        activePopups.RemoveAt(lastIndex);
        panel.Hide();
    }

    private void OnGameLoadingStarted() {
        foreach (UIPanel panel in panels) {
            if (!panel.IsOpen) { continue; }
            panel.Hide();
        }
    }

    private void UpdateBlocker() {
        blockerCanvas.enabled = false;
        for (int i = activePopups.Count - 1; i >= 0; i--) {
            UIPanel panel = activePopups[i];
            if (!panel.UsesBackgroundBlocker) { continue; }

            blockerCanvas.sortingOrder = panel.Canvas.sortingOrder - 1;
            blockerCanvas.enabled = true;
            break;
        }
    }

    public T GetPanel<T>() where T : UIPanel {
        T panel = (T)panels.FirstOrDefault(x => x is T);
        if (!panel) {
            Debug.LogError($"Panel of type {typeof(T)} not found! Make sure it's set up as a child of {GetType()}.");
        }
        return panel;
    }

    public UIPanel DebugGetFirstOpenPanel() {
        for (int i = panels.Length - 1; i >= 0; i--) {
            if (panels[i].IsOpen) {
                return panels[i];
            }
        }
        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This layer controls all Windows.
/// Windows are Screens that follow a history and a queue, and are displayed one at a time.
/// This also includes pop-up windows.
/// </summary>
public class WindowUILayer : UILayer<IWindowController>
{
    [SerializeField]
    private WindowParaLayer priorityParaLayer = null;

    public IWindowController CurrentWindow { get; private set; }

    private Queue<WindowHistoryEntry> windowQueue;
    private Stack<WindowHistoryEntry> windowHistory;

    public event Action RequestScreenBlock;
    public event Action RequestScreenUnblock;

    private HashSet<IUIScreenController> screensTransitioning;

    public override void Initialize()
    {
        base.Initialize();
        registeredScreens = new Dictionary<string, IWindowController>();
        windowQueue = new Queue<WindowHistoryEntry>();
        windowHistory = new Stack<WindowHistoryEntry>();
        screensTransitioning = new HashSet<IUIScreenController>();
    }

    protected override void ProcessScreenRegister(string screenID, IWindowController controller)
    {
        base.ProcessScreenRegister(screenID, controller);
        controller.OpeningTransitionFinished += OnOpeningAnimationFinished;
        controller.ClosingTransitionFinished += OnClosingAnimationFinished;
        controller.CloseRequest += OnCloseRequestedByWindow;
    }
    protected override void ProcessScreenUnregister(string screenID, IWindowController controller)
    {
        base.ProcessScreenUnregister(screenID, controller);
        controller.OpeningTransitionFinished -= OnOpeningAnimationFinished;
        controller.ClosingTransitionFinished -= OnClosingAnimationFinished;
        controller.CloseRequest -= OnCloseRequestedByWindow;
    }

    private bool IsScreenTransitionInProgress
    {
        get { return screensTransitioning.Count != 0; }
    }

    public override void ReparentScreen(IUIScreenController controller, Transform screenTransform)
    {
        IWindowController window = controller as IWindowController;

        if (window == null)
            Debug.Log("Screen " + screenTransform.name + " is not a Window!");
        else
        {
            if (window.IsPopup)
            {
                priorityParaLayer.AddScreen(screenTransform);
                return;
            }
        }

        base.ReparentScreen(controller, screenTransform);
    }

    private void DoShow(IWindowController screen, IWindowProperties properties)
    {
        DoShow(new WindowHistoryEntry(screen, properties));
    }

    private void DoShow(WindowHistoryEntry windowEntry)
    {
        if (CurrentWindow == windowEntry.Screen)
            Debug.Log("The requested Window ID " + windowEntry.Screen.screenID + " is already open!");
        else if (CurrentWindow != null && CurrentWindow.HideOnForegroundLost && !windowEntry.Screen.IsPopup)
            CurrentWindow.Hide();

        windowHistory.Push(windowEntry);
        AddTransition(windowEntry.Screen);

        if (windowEntry.Screen.IsPopup)
            priorityParaLayer.DarkenBG();

        windowEntry.Show();

        CurrentWindow = windowEntry.Screen;
    }

    #region Hide/Show Screen
    public override void HideScreen(IWindowController screen)
    {
        if (screen == CurrentWindow)
        {
            windowHistory.Pop();
            AddTransition(screen);
            screen.Hide();

            CurrentWindow = null;

            if (windowQueue.Count > 0)
                ShowNextInQueue();
            else
                ShowPreviousInHistory();
        }
        else
            Debug.Log("Hide Requested on Window ID " + CurrentWindow.screenID + " is not currently opened window!");
    }

    public override void HideAllScreens(bool animateWhenHiding = true)
    {
        base.HideAllScreens(animateWhenHiding);
        CurrentWindow = null;
        priorityParaLayer.RefreshDarken();
        windowHistory.Clear();
    }

    public override void ShowScreen(IWindowController screenToShow)
    {
        ShowScreen<IWindowProperties>(screenToShow, null);
    }

    public override void ShowScreen<TProps>(IWindowController screen, TProps properties)
    {
        IWindowProperties windowProperties = properties as IWindowProperties;

        if (ShouldEnqueue(screen, windowProperties))
            EnqueueWindow(screen, properties);
        else
            DoShow(screen, windowProperties);
    }
    #endregion

    #region Window Queue and Window History
    private void EnqueueWindow<TProp>(IWindowController screen, TProp properties) where TProp : IScreenProperties
    {
        windowQueue.Enqueue(new WindowHistoryEntry(screen, (IWindowProperties)properties));
    }

    private bool ShouldEnqueue(IWindowController controller, IWindowProperties windowProp)
    {
        if (CurrentWindow == null && windowQueue.Count == 0)
            return false;

        if (windowProp != null && windowProp.SupressPrefabProperties)
            return windowProp.QueuePriority != WindowPriority.ForceForeground;

        if (controller.QueuePriority != WindowPriority.ForceForeground)
            return true;

        return false;
    }

    private void ShowPreviousInHistory()
    {
        if (windowHistory.Count > 0)
        {
            WindowHistoryEntry window = windowHistory.Pop();
            DoShow(window);
        }
    }
    private void ShowNextInQueue()
    {
        if (windowQueue.Count > 0)
        {
            WindowHistoryEntry window = windowQueue.Dequeue();
            DoShow(window);
        }
    }
    #endregion

    #region Events
    private void OnOpeningAnimationFinished(IUIScreenController screen)
    {
        RemoveTransition(screen);
    }

    private void OnClosingAnimationFinished(IUIScreenController screen)
    {
        RemoveTransition(screen);

        var window = screen as IWindowController;
        if (window.IsPopup)
            priorityParaLayer.RefreshDarken();
    }

    private void OnCloseRequestedByWindow(IUIScreenController screen)
    {
        HideScreen(screen as IWindowController);
    }
    #endregion

    #region Add/Remove Transitions
    private void AddTransition(IUIScreenController screen)
    {
        screensTransitioning.Add(screen);
        RequestScreenBlock?.Invoke();
    }

    private void RemoveTransition(IUIScreenController screen)
    {
        screensTransitioning.Remove(screen);
        if(!IsScreenTransitionInProgress)
            RequestScreenUnblock?.Invoke();
    }
    #endregion
}

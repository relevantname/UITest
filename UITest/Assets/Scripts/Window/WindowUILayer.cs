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

    #region Hide/Show Screen
    public override void HideScreen(IWindowController screen)
    {
        throw new NotImplementedException();
    }

    public override void ShowScreen(IWindowController screenToShow)
    {
        throw new NotImplementedException();
    }

    public override void ShowScreen<TProps>(IWindowController screen, TProps properties)
    {
        
    }
    #endregion
}

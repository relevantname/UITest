using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base interface for all screen controllers. All screens must implement this interface.
/// </summary>
public interface IUIScreenController
{
    // Unique ID for each screen.
    string screenID { get; set; }

    bool IsVisible { get; set; }

    void Show(IScreenProperties properties = null);
    void Hide(bool animate = true);

    Action<IUIScreenController> OpeningTransitionFinished { get; set; }
    Action<IUIScreenController> ClosingTransitionFinished { get; set; }
    Action<IUIScreenController> CloseRequest { get; set; }
    Action<IUIScreenController> ScreenDestroyed { get; set; }
}

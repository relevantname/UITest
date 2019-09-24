using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base interface for window properties.
/// </summary>
public interface IWindowProperties : IScreenProperties
{
    WindowPriority QueuePriority { get; set; }

    // this defines if this window should be hidden when another Window is opened. In practice, this defines if you have a visible "stack" of windows, or if opening a new one hides the current one.
    bool HideOnForegroundLost { get; set; }

    // this defines if the window should work as a Pop-up. If marked, this will be parented to the WindowPriorityLayer, and it will automatically have a darkened background displayed under it.
    bool IsPopup { get; set; }

    bool SupressPrefabProperties { get; set; }
}

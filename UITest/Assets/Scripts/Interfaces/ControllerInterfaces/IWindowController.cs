using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindowController : IUIScreenController
{
    WindowPriority QueuePriority { get; }
    bool HideOnForegroundLost { get; }
    bool IsPopup { get; }    
}

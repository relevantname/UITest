using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPanelController : IUIScreenController
{
    PanelPriority Priority { get; }
}

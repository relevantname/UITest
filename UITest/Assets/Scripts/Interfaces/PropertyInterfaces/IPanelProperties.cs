using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base interface for all panel properties.
/// </summary>
public interface IPanelProperties : IScreenProperties
{
    PanelPriority Priority { get; set; }
}

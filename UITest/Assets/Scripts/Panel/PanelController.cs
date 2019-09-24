using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for panels that need no special Properties.
/// </summary>
public abstract class PanelController : PanelController<PanelProperties> { }

public abstract class PanelController<TProps> : UIScreenController<TProps>, IPanelController where TProps : IPanelProperties
{
    public PanelPriority Priority
    {
        get
        {
            if (ScreenProperties != null)
                return ScreenProperties.Priority;
            else
                return PanelPriority.None;
        }
    }

    protected sealed override void SetProperties(TProps properties)
    {
        base.SetProperties(properties);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowController : WindowController<WindowProperties> { }

public abstract class WindowController<TProps> : UIScreenController<TProps>, IWindowController where TProps : IWindowProperties
{
    public bool HideOnForegroundLost
    {
        get
        {
            return ScreenProperties.HideOnForegroundLost;
        }
    }

    public bool IsPopup
    {
        get
        {
            return ScreenProperties.IsPopup;
        }
    }

    public WindowPriority QueuePriority
    {
        get
        {
            return ScreenProperties.QueuePriority;
        }
    }

    public virtual void UI_Close()
    {
        CloseRequest(this);
    }

    protected sealed override void SetProperties(TProps properties)
    {
        // If properties are set on the prefab, then copy values.
        if(properties != null)
        {
            if (!properties.SupressPrefabProperties)
            {
                properties.HideOnForegroundLost = ScreenProperties.HideOnForegroundLost;
                properties.QueuePriority = ScreenProperties.QueuePriority;
                properties.IsPopup = ScreenProperties.IsPopup;
            }
        }
    }

    protected override void HierarchyFixOnShow()
    {
        transform.SetAsLastSibling();
    }
}

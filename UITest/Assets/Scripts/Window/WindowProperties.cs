using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WindowProperties : IWindowProperties
{
    [SerializeField]
    protected bool hideOnForegroundLost = true;
    [SerializeField]
    protected bool isPopUp = false;
    [SerializeField]
    protected WindowPriority queuePriority = WindowPriority.ForceForeground;

    public bool HideOnForegroundLost
    {
        get { return hideOnForegroundLost; }
        set { hideOnForegroundLost = value; }
    }
    public bool IsPopup
    {
        get { return isPopUp; }
        set { isPopUp = value; }
    }
    public WindowPriority QueuePriority
    {
        get { return queuePriority; }
        set { queuePriority = value; }
    }
    public bool SupressPrefabProperties
    {
        get;
        set;
    }

    public WindowProperties()
    {
        hideOnForegroundLost = true;
        queuePriority = WindowPriority.ForceForeground;
        isPopUp = false;
    }

    public WindowProperties(bool suppressPrefabProperties = false)
    {
        QueuePriority = WindowPriority.ForceForeground;
        HideOnForegroundLost = false;
        SupressPrefabProperties = suppressPrefabProperties;
    }

    public WindowProperties(WindowPriority priority, bool hideOnForegroundLost = false, bool suppressPrefabProperties = false)
    {
        QueuePriority = priority;
        HideOnForegroundLost = hideOnForegroundLost;
        SupressPrefabProperties = suppressPrefabProperties;
    }
}

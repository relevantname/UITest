using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PanelPriorityLayerListEntry
{
    [SerializeField]
    private PanelPriority priority;

    [SerializeField]
    private Transform targetParent;

    public Transform TargetParent
    {
        get { return targetParent; }
        set { targetParent = value; }
    }

    public PanelPriority Priority
    {
        get { return priority; }
        set { priority = value; }
    }

    public PanelPriorityLayerListEntry(PanelPriority priority, Transform targetParent)
    {
        this.priority = priority;
        this.targetParent = targetParent;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PanelProperties : IPanelProperties
{
    [SerializeField]
    private PanelPriority priority;

    public PanelPriority Priority
    {
        get
        {
            return priority;
        }

        set
        {
            priority = value;
        }
    }
}

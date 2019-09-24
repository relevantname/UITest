using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Layer controls Panels.
/// Panels are screens that have no history or queuing.
/// ex: a HUD, mini-map etc.
/// </summary>
public class PanelUILayer : UILayer<IPanelController>
{
    [SerializeField]
    private PanelPriorityLayerList priorityLayers = null;

    #region Hide/Show Screen
    public override void ShowScreen(IPanelController screenToShow)
    {
        screenToShow.Show();
    }

    public override void ShowScreen<TProps>(IPanelController screenToShow, TProps properties)
    {
        screenToShow.Show(properties);
    }

    public override void HideScreen(IPanelController screenToHide)
    {
        screenToHide.Hide();
    }
    #endregion

    public bool IsPanelVisible(string panelID)
    {
        IPanelController panel;
        if (registeredScreens.TryGetValue(panelID, out panel))
            return panel.IsVisible;

        return false;
    }

    private void ReparentToParaLayer(PanelPriority priority, Transform screenTransform)
    {
        Transform transform;
        if (!priorityLayers.ParaLayerLookupTable.TryGetValue(priority, out transform))
            transform = this.transform;

        screenTransform.SetParent(transform, false);
    }
}

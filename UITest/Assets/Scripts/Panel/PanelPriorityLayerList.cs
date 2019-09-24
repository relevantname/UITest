using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PanelPriorityLayerList
{
    [SerializeField]
    private List<PanelPriorityLayerListEntry> paraLayers = null;

    private Dictionary<PanelPriority, Transform> lookupTable;

    public Dictionary<PanelPriority, Transform> ParaLayerLookupTable
    {
        get
        {
            if (lookupTable == null || lookupTable.Count == 0)
                CacheLookupTable();

            return lookupTable;
        }
    }

    private void CacheLookupTable()
    {
        lookupTable = new Dictionary<PanelPriority, Transform>();
        for (int i = 0; i < paraLayers.Count; i++)
            lookupTable.Add(paraLayers[i].Priority, paraLayers[i].TargetParent);
    }

    public PanelPriorityLayerList(List<PanelPriorityLayerListEntry> entries)
    {
        paraLayers = entries;
    }
}

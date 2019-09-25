using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Centeralize Location of All UI System
/// </summary>
public class UIFrame : MonoBehaviour
{
    [SerializeField] private bool initializeOnAwake = true;

    private PanelUILayer panelLayer;
    private WindowUILayer windowLayer;

    private Canvas mainCanvas;
    private GraphicRaycaster graphicRaycaster;

    public Canvas MainCanvas
    {
        get { if (mainCanvas == null) mainCanvas = GetComponent<Canvas>(); return mainCanvas; }
    }

    public Camera UICamera
    {
        get { return MainCanvas.worldCamera; }
    }

    private void Awake()
    {
        if (initializeOnAwake)
            Initialize();
    }

    public virtual void Initialize()
    {
        if(panelLayer == null)
        {
            panelLayer = gameObject.GetComponentInChildren<PanelUILayer>(true);

            if (panelLayer == null)
                Debug.Log("UI Frame lacks Panel Layer!");
            else
                panelLayer.Initialize();
        }

        if(windowLayer == null)
        {
            windowLayer = gameObject.GetComponentInChildren<WindowUILayer>(true);
            if (panelLayer == null)
                Debug.Log("UI Frame lacks Window Layer!");
            else
            {
                windowLayer.Initialize();
                windowLayer.RequestScreenBlock += OnRequestScreenBlock;
                windowLayer.RequestScreenUnblock += OnRequestScreenUnblock;
            }
        }

        graphicRaycaster = MainCanvas.GetComponent<GraphicRaycaster>();
    }

    #region Show/Hide Panel
    public void ShowPanel(string screenID)
    {
        panelLayer.ShowScreenByID(screenID);
    }

    public void ShowPanel<T>(string screenID, T properties) where T : IPanelProperties
    {
        panelLayer.ShowScreenByID(screenID, properties);
    }

    public void HidePanel(string screenID)
    {
        panelLayer.HideScreenByID(screenID);
    }
    #endregion

    #region Show/Hide Window
    public void OpenWindow(string screenID)
    {
        windowLayer.ShowScreenByID(screenID);
    }

    public void OpenWindow<T>(string screenID, T properties) where T : IWindowProperties
    {
        windowLayer.ShowScreenByID(screenID, properties);
    }

    public void CloseWindow(string screenID)
    {
        windowLayer.HideScreenByID(screenID);
    }

    public void CloseCurrentWindow()
    {
        if (windowLayer.CurrentWindow != null)
            CloseWindow(windowLayer.CurrentWindow.screenID);
    }
    #endregion

    #region Open-Close Canvas Interactability
    private void OnRequestScreenBlock()
    {
        if (graphicRaycaster != null)
            graphicRaycaster.enabled = false;
    }
    private void OnRequestScreenUnblock()
    {
        if (graphicRaycaster != null)
            graphicRaycaster.enabled = true;
    }
    #endregion
}

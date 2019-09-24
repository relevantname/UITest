using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base implementation for UI Screens.
/// But when creating a panel or window use PanelController or WindowController.
/// </summary>
/// <typeparam name="TProps"></typeparam>
public abstract class UIScreenController<TProps> : MonoBehaviour, IUIScreenController where TProps : IScreenProperties
{
    [SerializeField]
    private TransitionComponent openingAnim;

    [SerializeField]
    private TransitionComponent closingAnim;

    // Properties for screen (either panel properties or window properties)
    private TProps screenProperties;

    // It should same name as the screen's prefab.
    public string screenID { get; set; }
    public bool IsVisible { get; set; }

    public TransitionComponent OpeningAnim { get { return openingAnim; } set { openingAnim = value; } }
    public TransitionComponent ClosingAnim { get { return closingAnim; } set { closingAnim = value; } }

    public TProps ScreenProperties { get { return screenProperties; } set { screenProperties = value; } }

    protected virtual void Awake()
    {
        AddListeners();
    }

    protected virtual void OnDestroy()
    {
        ScreenDestroyed?.Invoke(this);

        OpeningTransitionFinished = null;
        ClosingTransitionFinished = null;
        CloseRequest = null;
        ScreenDestroyed = null;
        RemoveListeners();
    }

    protected virtual void AddListeners()
    {

    }

    protected virtual void RemoveListeners()
    {

    }

    /// <summary>
    /// When properties are set for this screen, this method is called.
    /// </summary>
    protected virtual void OnPropertiesSet()
    {

    }

    /// <summary>
    /// When the screen animates out, this method is called.
    /// </summary>
    protected virtual void WhileHiding()
    {

    }

    protected virtual void SetProperties(TProps properties)
    {
        this.screenProperties = properties;
    }
    protected virtual void HierarchyFixOnShow() { }

    public void Hide(bool animate = true)
    {
        DoAnimation(animate ? closingAnim : null, OnOpeningTransitionFinished, false);
        WhileHiding();
    }

    /// <summary>
    /// Show this screen with specified properties.
    /// </summary>
    /// <param name="properties"></param>
    public void Show(IScreenProperties properties = null)
    {
        if(properties != null)
        {
            if (properties is TProps)
                SetProperties((TProps)properties);
            else
                return;
        }

        HierarchyFixOnShow();
        OnPropertiesSet();

        if (!gameObject.activeSelf)
            DoAnimation(openingAnim, OnClosingTransitionFinished, true);
        else
        {
            OpeningTransitionFinished?.Invoke(this);
        }
    }

    private void DoAnimation(TransitionComponent caller, Action callWhenFinished, bool isVisible)
    {
        if(caller == null)
        {
            gameObject.SetActive(isVisible);
            callWhenFinished?.Invoke();
        }
        else
        {
            if (isVisible && !gameObject.activeSelf)
                gameObject.SetActive(true);

            caller.Animate(this.transform, callWhenFinished);
        }
    }
    
    private void OnOpeningTransitionFinished()
    {
        IsVisible = true;

        OpeningTransitionFinished?.Invoke(this);
    }

    private void OnClosingTransitionFinished()
    {
        IsVisible = false;
        gameObject.SetActive(false);

        ClosingTransitionFinished?.Invoke(this);
    }

    // Screen can fire this event to request its responsible layer to close it.
    public Action<IUIScreenController> CloseRequest{ get; set; }

    public Action<IUIScreenController> OpeningTransitionFinished { get; set; }
    public Action<IUIScreenController> ClosingTransitionFinished{ get; set; }

    public Action<IUIScreenController> ScreenDestroyed { get; set; }
}

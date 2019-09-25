using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for UI layers.
/// </summary>
public abstract class UILayer<TScreen> : MonoBehaviour where TScreen : IUIScreenController
{
    // Key => ScreenID
    // Value => IUIScreenController
    protected Dictionary<string, TScreen> registeredScreens;

    public virtual void Initialize()
    {
        registeredScreens = new Dictionary<string, TScreen>();
    }

    public virtual void ReparentScreen(IUIScreenController controller, Transform screenTransform)
    {
        screenTransform.SetParent(transform, false);
    }

    #region Show Screen
    /// <summary>
    /// Shows the given screen.
    /// </summary>
    /// <param name="screenToShow"></param>
    public abstract void ShowScreen(TScreen screenToShow);

    public abstract void ShowScreen<TProps>(TScreen screen, TProps properties) where TProps : IScreenProperties;

    public void ShowScreenByID(string screenID)
    {
        TScreen screen;
        if (registeredScreens.TryGetValue(screenID, out screen))
            ShowScreen(screen);
        else
            Debug.Log("Couldn't find a screen with this screen Id => " + screenID);
    }

    public void ShowScreenByID<TProps>(string screenID, TProps properties) where TProps : IScreenProperties
    {
        TScreen screen;
        if (registeredScreens.TryGetValue(screenID, out screen))
            ShowScreen<TProps>(screen, properties);
        else
            Debug.Log("Couldn't find a screen with this screen Id => " + screenID);
    }
    #endregion

    #region Hide Screen
    /// <summary>
    /// Hides the given screen.
    /// </summary>
    /// <param name="screen"></param>
    public abstract void HideScreen(TScreen screen);

    public void HideScreenByID(string screenID)
    {
        TScreen screen;
        if (registeredScreens.TryGetValue(screenID, out screen))
            HideScreen(screen);
        else
            Debug.Log("Couldn't find a screen with this screen Id => " + screenID);
    }

    public virtual void HideAllScreens(bool animateWhenHiding = true)
    {
        foreach (TScreen screen in registeredScreens.Values)
            screen.Hide(animateWhenHiding);
    }
    #endregion

    #region Register/Unregister
    public void RegisterScreen(string screenID, TScreen screen)
    {
        if (!registeredScreens.ContainsKey(screenID))
        {
            ProcessScreenRegister(screenID, screen);
        }
        else
            Debug.Log("This screen already registered! Screen ID => " + screenID);
    }

    public void UnregisterScreen(string screenID, TScreen screen)
    {
        if (registeredScreens.ContainsKey(screenID))
        {
            ProcessScreenUnregister(screenID, screen);
        }
        else
            Debug.Log("Screen couln't found! Screen ID => " + screenID);
    }

    protected virtual void ProcessScreenRegister(string screenID, TScreen screen)
    {
        screen.screenID = screenID;
        registeredScreens.Add(screenID, screen);
        screen.ScreenDestroyed += OnScreenDestroyed;
    }

    protected virtual void ProcessScreenUnregister(string screenID, TScreen screen)
    {
        TScreen screenToUnregister = registeredScreens[screenID];
        screenToUnregister.ScreenDestroyed -= OnScreenDestroyed;
        registeredScreens.Remove(screenID);
    }

    public bool IsScreenRegistered(string screenID)
    {
        return registeredScreens.ContainsKey(screenID);
    }
    #endregion

    private void OnScreenDestroyed(IUIScreenController screen)
    {
        if (!string.IsNullOrEmpty(screen.screenID) && registeredScreens.ContainsKey(screen.screenID))
            UnregisterScreen(screen.screenID, (TScreen)screen);
    }
}

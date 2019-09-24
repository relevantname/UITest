using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Screens use TranstionComponent to animate their in and out transitions.
/// </summary>
public abstract class TransitionComponent : MonoBehaviour
{
    /// <summary>
    /// Animate the specified target transform and execute CallWhenFinished when the animation is done.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="callWhenFinished"></param>
    public abstract void Animate(Transform target, Action callWhenFinished);
}

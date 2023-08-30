using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a basic interactive object.
/// <para /> Inherits from MonoBehaviour.
/// </summary>
public class BasicInteractive : MonoBehaviour
{
    // Event that is invoked when the object is interacted with.
    public UnityEvent OnInteract;

    /// <summary>
    /// Returns the command associated with the interaction.
    /// </summary>
    /// <returns>The command associated with the interaction. Returns null by default.</returns>
    public virtual ICommand QueryInteraction()
    {
        OnInteract.Invoke();

        return null;
    }
}


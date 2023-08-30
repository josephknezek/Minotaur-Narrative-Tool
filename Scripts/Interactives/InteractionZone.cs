using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents an interaction zone that sets the current interactive object for a player
/// when the player enters the trigger area and sets it to null when they exit.
/// <para /> Inherits from MonoBehaviour.
/// </summary>
public class InteractionZone : MonoBehaviour
{
    // The interactive object to set for the player.
    public BasicInteractive InteractiveToSet;

    public UnityEvent OpenEvent = new UnityEvent();

    public UnityEvent CloseEvent = new UnityEvent();

    private PlayerController player;

    private void OnDisable()
    {
        CloseEvent.Invoke();
        if (player != null)
        {
            player.OnInteractiveChanged.RemoveListener(WaitForTogglePrompt);
            player.RemoveInteractive(InteractiveToSet);
        }
        
        player = null;
    }

    /// <summary>
    /// Called when a 2D collider enters the trigger area.
    /// Sets the current interactive object for the player.
    /// </summary>
    /// <param name="collision">The collider that entered the trigger area.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out player))
            return;

        player.AddInteractive(InteractiveToSet);

        if (player.CurrentInteractive == InteractiveToSet)
        {
            OpenEvent.Invoke();
            player.OnInteractiveChanged.AddListener(WaitForTogglePrompt);
        }
    }

    /// <summary>
    /// Called when a 2D collider exits the trigger area.
    /// Sets the current interactive object for the player to null.
    /// </summary>
    /// <param name="collision">The collider that exited the trigger area.</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out player))
            return;

        if (player != null)
        {
            player.RemoveInteractive(InteractiveToSet);
            CloseEvent.Invoke();
            player.OnInteractiveChanged.RemoveListener(WaitForTogglePrompt);

            player = null;
        }
    }

    private void WaitForTogglePrompt()
    {
        if (player == null)
            return;

        if (player.CurrentInteractive == InteractiveToSet)
            OpenEvent.Invoke();
        else
            CloseEvent.Invoke();
    }

}


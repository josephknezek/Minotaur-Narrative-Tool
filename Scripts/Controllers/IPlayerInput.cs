using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Interface for handling player input.
/// </summary>
public interface IPlayerInput
{
    /// <summary>
    /// Called when the player attempts to interact with an object.
    /// </summary>
    public void OnInteract();

    /// <summary>
    /// Called when the player moves, passing in the input value.
    /// </summary>
    /// <param name="value">The input value.</param>
    public void OnMovement(InputValue value);

    /// <summary>
    /// Called when the player presses the continue button to advance dialogues or cutscenes.
    /// </summary>
    public void OnContinue();

    /// <summary>
    /// Called when the player presses TAB.
    /// </summary>
    public void OnOpenQuestUI();

    /// <summary>
    /// Called when the player presses TAB.
    /// </summary>
    public void OnCloseQuestUI();
}


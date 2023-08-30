using UnityEngine;

/// <summary>
/// Represents an interface for commands.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="invoker">
    /// The MonoBehaviour that invoked the command.
    /// If null, the command will be executed without any invoker.
    /// </param>
    public void Execute(MonoBehaviour invoker = null);
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;

/// <summary>
/// Controls the movement and interaction of the player character.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IPlayerInput
{
    public const string GameplayInputName = "Adventure";

    public static PlayerController Instance { get => _instance; }
    private static PlayerController _instance;

    [Header("Controller Variables")]
    public float Movespeed = 5f;

    [Header("Events")]
    public BasicSOEvent DialogueContinueEvent;
    public BasicSOEvent OpenQuestUIEvent;
    public BasicSOEvent CloseQuestUIEvent;

    public BasicInteractive CurrentInteractive
    {
        get => _currentInteractive;
        set
        {
            if (value == _currentInteractive)
                return;

            _currentInteractive = value;
            OnInteractiveChanged.Invoke();
        }
    }
    public UnityEvent OnInteractiveChanged { get; set; } = new();

    private List<BasicInteractive> _interactives = new();

    [HideInInspector]
    public PlayerInput Input;

    // Private components
    private SpriteRenderer _render;
    private Animator _anim;
    private Rigidbody2D _rb; 
    private BasicInteractive _currentInteractive;
    private ICommand _localCommand;

    // Private values
    private Vector2 _movement = Vector2.zero;


    private void OnValidate()
    {
        if (Input == null)
            Input = GetComponent<PlayerInput>();
    }

    /// <summary>
    /// Get the required components
    /// </summary>
    void Awake()
    {
        _instance = this;

        _render = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update the movement of the player
    /// </summary>
    void Update()
    {
        _rb.velocity = _movement.normalized * Movespeed;

        _anim.SetBool("Walking", _movement.magnitude > 0);

        if (_movement.x > 0)
        {
            _render.flipX = false;
        }
        else if (_movement.x < 0)
        {
            _render.flipX = true;
        }
    }


    /// <summary>
    /// Interact with the current interactive object
    /// </summary>
    public void OnInteract()
    {
        // If we can't interact, don't
        if (_currentInteractive == null)
            return;

        // Interact with the object and get a local command (if any) to execute
        _localCommand = _currentInteractive.QueryInteraction();

        // If there is no local command, do not execute it
        if (_localCommand == null)
            return;

        // Execute the command, but make sure we only do it once.
        _localCommand.Execute(this);
        _localCommand = null;
    }

    /// <summary>
    /// Get the movement input from the player
    /// </summary>
    /// <param name="value">The value of the input</param>
    public void OnMovement(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }

    /// <summary>
    /// Trigger the event for continuing dialogue
    /// </summary>
    public void OnContinue()
    {
        DialogueContinueEvent.Invoke();
    }

    public void OnOpenQuestUI()
    {
        OpenQuestUIEvent.Invoke();
    }

    public void OnCloseQuestUI()
    {
        CloseQuestUIEvent.Invoke();
    }

    public void AddInteractive(BasicInteractive interactive)
    {
        _interactives.Add(interactive);
        ChangedInteractiveList();
    }

    public void RemoveInteractive(BasicInteractive interactive)
    {
        if (_interactives.Contains(interactive))
        {
            _interactives.Remove(interactive);
            ChangedInteractiveList();
        }
    }

    private void ChangedInteractiveList()
    {
        if (_interactives.Any())
            CurrentInteractive = _interactives.Last();
        else
            CurrentInteractive = null;
    }
}


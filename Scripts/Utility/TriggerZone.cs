using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider2D))]
public class TriggerZone : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField]
    private bool _requirePlayer = false;

    [SerializeField]
    private bool _oneShot = false;

    [Header("Events")]
    public UnityEvent OnEnter = new();
    public UnityEvent OnStay = new();
    public UnityEvent OnExit = new();

    private delegate void EventCallback();

    [SerializeField, HideInInspector]
    private Collider2D _myCollider;

    private void OnValidate()
    {
        if (_myCollider == null)
        {
            _myCollider = GetComponent<Collider2D>();
        }
        else if (!_myCollider.isTrigger)
        {
            _myCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoCollision(collision, OnEnter.Invoke);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        DoCollision(collision, OnStay.Invoke);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DoCollision(collision, OnExit.Invoke);
    }

    private void DoCollision(Collider2D collision, EventCallback onComplete)
    {
        if (CheckPlayer(collision))
        {
            onComplete();

            CheckOneShot();
        }
    }

    private bool CheckPlayer(Collider2D collision)
    {
        if (!_requirePlayer)
            return true;

        return collision.CompareTag("Player");
    }

    private void CheckOneShot()
    {
        if (_oneShot)
            enabled = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionalMacro : MonoBehaviour
{

    [SerializeField]
    private List<Conditional> _conditionals = new();

    [SerializeField]
    private UnityEvent _onConfirmed = new();

    [SerializeField]
    private UnityEvent _onFail = new();


    private void Start()
    {
        foreach (var item in _conditionals)
        {
            item.Initialize();
        }
    }

    public void ConfirmEvent()
    {
        foreach (var item in _conditionals)
        {
            if (!item.CurrentCondition)
            {
                _onFail.Invoke();
                return;
            }
        }

        _onConfirmed.Invoke();
    }

}

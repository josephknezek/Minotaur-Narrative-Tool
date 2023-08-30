using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

[Serializable, Inspectable]
public class Remember
{
    public enum Operation { SetTo, Add, Subtract, MultiplyBy, DivideBy }

    public BlackboardVariable Variable;
    public Operation MyOperation;
    public float Value;

    public void PerformOperation()
    {
        switch (MyOperation)
        {
            case Operation.SetTo:
                Variable.Value = (int)Value;
                break;
            case Operation.Add:
                Variable.Value += (int)Value;
                break;
            case Operation.Subtract:
                Variable.Value -= (int)Value;
                break;

            // Allow floating-point operations (such as "Increase by 20%")
            case Operation.MultiplyBy:
                Variable.Value = (int)(Variable.Value * Value);
                break;
            case Operation.DivideBy:
                Variable.Value = (int)(Variable.Value / Value);
                break;
            default:
                break;
        }
    }
}

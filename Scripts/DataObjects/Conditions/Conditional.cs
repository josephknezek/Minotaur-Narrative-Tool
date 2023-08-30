using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
using System;

[Inspectable, Serializable]
public class Conditional
{
    // Enumerator for different evaluators
    public enum Evaluator { Equals, NotEquals, GreaterThan, LessThan, GreaterOrEqual, LessOrEqual }

    // The blackboard variable to reference
    public BlackboardVariable BlackboardReference;

    // The selected evaluator
    public Evaluator MyEvaluator;

    // The value to compare against
    public int ComparisonValue;

    // Event triggered when the condition changes
    public UnityEvent OnConditionChanged;

    // The current evaluated condition
    public bool CurrentCondition
    {
        get => _condition;
        private set
        {
            if (_condition == value)
                return;

            _condition = value;
            OnConditionChanged.Invoke();
        }
    }

    // Private field for the condition
    private bool _condition = false;

    // Constructor for Conditional class
    public Conditional(BlackboardVariable variable, Evaluator Condition, int Comparison)
    {
        // Initialize fields
        BlackboardReference = variable;
        MyEvaluator = Condition;
        ComparisonValue = Comparison;
    }

    // Initialize the conditional
    public void Initialize()
    {
        // Initialize our starting condition
        Evaluate(BlackboardReference.Value);

        // Re-evaluate every time the value changes
        BlackboardReference.OnValueChanged.AddListener(Evaluate);
    }

    // Evaluate the condition based on the provided value
    public void Evaluate(int value)
    {
        // Perform the evaluation based on the selected evaluator
        CurrentCondition = MyEvaluator switch
        {
            Evaluator.Equals => value == ComparisonValue,
            Evaluator.NotEquals => value != ComparisonValue,
            Evaluator.GreaterThan => value > ComparisonValue,
            Evaluator.LessThan => value < ComparisonValue,
            Evaluator.GreaterOrEqual => value >= ComparisonValue,
            Evaluator.LessOrEqual => value <= ComparisonValue,
            _ => false
        };
    }

    // Override the ToString() method to provide a formatted string representation of the condition
    public override string ToString()
    {
        return $"Condition: <b>{BlackboardReference.name} <i>({BlackboardReference.Value})</i> {MyEvaluator} {ComparisonValue}</b>\n" +
            $"Currently evaluated to <b>{CurrentCondition}</b>.";
    }
}

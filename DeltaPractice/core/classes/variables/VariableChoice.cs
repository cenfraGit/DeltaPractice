using System;

namespace core.classes.variables;

/// <summary>
/// Represents a random choice from an array of choices (number or strings).
/// </summary>
public class VariableChoice : IVariable<object>
{
    private object? _value;
    public object Value
    {
        get
        {
            if (this._value is null)
                Recalculate();
            return _value;
        }
        set => _value = value;
    }
    
    private object[] _choices;
    public object[] Choices
    {
        get => _choices;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            this._choices = value;
        }
    }
    
    public VariableChoice(object[] choices)
    {
        Choices = choices;
    }
    
    public void Recalculate()
    {
        Random random = new Random();
        int randomIdx = random.Next(0, Choices.Length);
        this.Value = Choices[randomIdx];
    }
}

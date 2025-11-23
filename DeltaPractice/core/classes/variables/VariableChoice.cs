using System;

namespace core.classes.variables;

/// <summary>
/// Represents a random choice from an array of choices (number or strings).
/// </summary>
public class VariableChoice : AVariable
{
    // ----------- fields and properties ----------- //

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

    // ---------------- constructors ---------------- //

    public VariableChoice(object[] choices) : base(VariableType.Choice)
    {
        Choices = choices;
    }

    // ------------------ methods ------------------ //

    public override void Recalculate()
    {
        Random random = new Random();
        int randomIdx = random.Next(0, Choices.Length);
        this.Value = Choices[randomIdx];
    }
}

using System;
using System.Numerics;

namespace core.classes.variables;

// --------------------------------------------------------------------------------
// AVariable
// --------------------------------------------------------------------------------

/// <summary>
/// Holds shared logic for all variables.
/// </summary>
public abstract class AVariable : IVariable
{
    // ----------- fields and properties ----------- //

    public VariableType Type { get; init; }

    private object? _value;
    public object? Value
    {
        get => _value;
        set => _value = value;
    }

    // ---------------- constructors ---------------- //

    public AVariable(VariableType variableType)
    {
        Type = variableType;
    }

    // ------------------- metods ------------------- //

    /// <summary>
    /// Recalculates (and reassigns) the Value of the variable.
    /// </summary>
    public abstract void Recalculate();
}

// --------------------------------------------------------------------------------
// AVariableNumeric
// --------------------------------------------------------------------------------

/// <summary>
/// Represents a random numeric value between the range of LowerLimit and UpperLimit.
/// Holds shared logic for integer and decimal variables.
/// Note: lower and upper limits will be cast to int when using the VariableInteger subclass.
/// </summary>
public abstract class AVariableNumeric : AVariable
{
    // ----------- fields and properties ----------- //

    private float _limitLower;
    private float _limitUpper;

    public float LimitLower
    {
        get => this._limitLower;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value > this._limitUpper)
                throw new ArgumentException($"Lower limit can't be greater than upper limit. ({value} > {_limitUpper}).");

            this._limitLower = value;
        }
    }

    public float LimitUpper
    {
        get => this._limitUpper;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value < this._limitLower)
                throw new ArgumentException($"Upper limit can't be less than lower limit. ({value} < {_limitLower}).");

            this._limitUpper = value;
        }
    }

    // ---------------- constructors ---------------- //

    protected AVariableNumeric(VariableType variableType, float limitLower, float limitUpper)
        : base(variableType)
    {
        ArgumentNullException.ThrowIfNull(limitLower);
        ArgumentNullException.ThrowIfNull(limitUpper);

        this._limitLower = limitLower;
        this._limitUpper = limitUpper;

        // confirm if values are valid
        this.LimitLower = limitLower;
        this.LimitUpper = limitUpper;
    }

    // ------------------ methods ------------------ //

    public abstract override void Recalculate();
}

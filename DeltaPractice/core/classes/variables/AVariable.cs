using System;
using System.Numerics;

namespace core.classes.variables;

/// <summary>
/// Represents a random numeric value between the range of LowerLimit and UpperLimit.
/// Holds shared logic for integer and decimal variables.
/// Note: lower and upper limits will be cast to int when using the VariableInteger subclass.
/// </summary>
public abstract class AVariableNumeric : IVariable
{
    // --------------------------------------------------------------------------------
    // fields
    // --------------------------------------------------------------------------------

    private float _limitLower;
    private float _limitUpper;

    // --------------------------------------------------------------------------------
    // properties
    // --------------------------------------------------------------------------------

    public abstract VariableType Type { get; }

    private object? _value;
    public object Value
    {
        get
        {
            if (this._value is null)
                Recalculate();
            return _value;
        }
        protected set => _value = value;
    }

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

    // --------------------------------------------------------------------------------
    // constructor
    // --------------------------------------------------------------------------------

    protected AVariableNumeric(float limitLower, float limitUpper)
    {
        ArgumentNullException.ThrowIfNull(limitLower);
        ArgumentNullException.ThrowIfNull(limitUpper);

        this._limitLower = limitLower;
        this._limitUpper = limitUpper;

        // confirm if values are valid
        this.LimitLower = limitLower;
        this.LimitUpper = limitUpper;
    }

    // --------------------------------------------------------------------------------
    // methods
    // --------------------------------------------------------------------------------

    /// <summary>
    /// Recalculates the Value of the variable.
    /// </summary>
    public abstract void Recalculate();
}

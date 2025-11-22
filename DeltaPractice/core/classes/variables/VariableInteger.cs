using System;

namespace core.classes.variables;

/// <summary>
/// Represents a random integer between the range of LowerLimit and UpperLimit
/// </summary>
public class VariableInteger : IVariable<int>
{
    // --------------------------------------------------------------------------------
    // fields
    // --------------------------------------------------------------------------------

    private int _limitLower;
    private int _limitUpper;

    // --------------------------------------------------------------------------------
    // properties
    // --------------------------------------------------------------------------------

    public int Value { get; set; }

    public int LimitLower
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

    public int LimitUpper
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

    public VariableInteger(int limitLower, int limitUpper)
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
    public void Recalculate()
    {
        Random random = new Random();
        this.Value = random.Next(this.LimitLower, this.LimitUpper + 1);
    }
}

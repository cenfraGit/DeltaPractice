using System;

namespace core.classes.variables;

/// <summary>
/// Represents a random integer between the range of LowerLimit and UpperLimit (inclusive).
/// </summary>
public class VariableInteger : AVariableNumeric
{
    public override VariableType Type { get; } = VariableType.Integer;

    public VariableInteger(float limitLower, float limitUpper) : base(limitLower, limitUpper)
    { }

    public override void Recalculate()
    {
        Random random = new Random();
        this.Value = (object)random.Next((int)this.LimitLower, (int)this.LimitUpper + 1);
    }
}

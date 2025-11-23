using System;

namespace core.classes.variables;

/// <summary>
/// Represents a random decimal between the range of LowerLimit and UpperLimit.
/// </summary>
public class VariableDecimal : AVariableNumeric
{
    // ---------------- constructors ---------------- //

    public VariableDecimal(float limitLower, float limitUpper) : base(VariableType.Decimal,
                                                                      limitLower, limitUpper)
    { }

    // ------------------ methods ------------------ //

    public override void Recalculate()
    {
        Random random = new Random();
        float randomVal = (float)random.NextDouble();
        float range = LimitUpper - LimitLower;
        float randomDouble = LimitLower + (range * randomVal);
        randomDouble = (float)Math.Round((decimal)randomDouble, 3);
        this.Value = (object)randomDouble;
    }
}

using System;

namespace core.classes;

public class Variable
{
    // variable represents a dynamic value in the problem. can be used to
    // generate dynamic questions that hold different answers based on the
    // random value of the variable.

    public enum VariableTypeEnum
    {
        Integer, // random int between LowerLimit and UpperLimit
        Decimal, // random float between LowerLimit and UpperLimit, rounded to RoundingPlaces
        Choice   // random value from Choices array.
    }

    // --------------------------------------------------------------------------------
    // fields
    // --------------------------------------------------------------------------------

    private string?           _variableName;
    private VariableTypeEnum? _variableType;
    private object?           _currentValue;
    public double?            _limitLower;     // integer, float
    public double?            _limitUpper;     // integer, float
    public int?               _roundingPlaces; // float
    public object[]?          _choices;        // choice (number, strings)

    // --------------------------------------------------------------------------------
    // properties
    // --------------------------------------------------------------------------------

    public string? VariableName
    {
        get => _variableName;
        set => _variableName = value;
    }
    
    public VariableTypeEnum? VariableType
    {
        get => _variableType;
        set => _variableType = value;
    }

    public object? CurrentValue
    {
        get
        {
            if (_currentValue is null)
                Recalculate();
            return _currentValue;
        }
        private set => _currentValue = value;
    }

    public double? LimitLower
    {
        get => _limitLower;
        set
        {
            if (value is null)
            {
                _limitLower = null;
                return;
            }
            // check if limit upper is set
            if (_limitUpper is not null)
            {
                // if new lower value is greater than the upper value, throw
                if (value.Value > _limitUpper.Value)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value), 
                        $"The lower limit ({value.Value}) cannot be greater than the current upper limit ({_limitUpper.Value})."
                    );
                }
            }
            _limitLower = value;
        }
    }

    public double? LimitUpper
    {
        get => _limitUpper;
        set
        {
            if (value is null)
            {
                _limitUpper = null;
                return;
            }
            // check if limit lower is set
            if (_limitLower is not null)
            {
                // if new upper value is less than the lower value, throw
                if (value.Value < _limitLower.Value)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value), 
                        $"The upper limit ({value.Value}) cannot be less than the current lower limit ({_limitLower.Value})."
                    );
                }
            }       
            _limitUpper = value;
        }
    }

    public int? RoundingPlaces
    {
        get => _roundingPlaces;
        set
        {
            if (value is null)
            {
                _roundingPlaces = null;
                return;
            }

            if (value.Value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), 
                    $"The rounding places cannot be less than 0 ({value.Value}).");
            }
                    
            
            _roundingPlaces = value;
        }
    }

    public object[]? Choices
    {
        get => _choices;
        set => _choices = value;
    }

    // --------------------------------------------------------------------------------
    // constructor
    // --------------------------------------------------------------------------------
    
    public Variable(string? variableName, VariableTypeEnum? variableType,
                    double? limitLower, double? limitUpper, int? roundingPlaces,
                    object[]? choices)
    {

        ArgumentNullException.ThrowIfNull(variableName);
        ArgumentNullException.ThrowIfNull(variableType);
        
        VariableName = variableName;
        VariableType = variableType;
        LimitLower = limitLower;
        LimitUpper = limitUpper;
        RoundingPlaces = roundingPlaces;
        Choices = choices;
    }

    /// <summary>
    /// Recalculates the CurrentValue of the variable.
    /// </summary>
    public void Recalculate()
    {
        // used by all three variable types
        Random random = new Random();

        switch (VariableType)
        {
            case VariableTypeEnum.Integer:
                if (LimitLower is null || LimitUpper is null)
                {
                    CurrentValue = null;
                    throw new InvalidOperationException("Required parameters are invalid: LimitLower or LimitUpper is null.");
                }
                int limitLower = (int?)LimitLower ?? 0;
                int limitUpper = (int?)LimitUpper ?? 0;
                CurrentValue = random.Next(limitLower, limitUpper + 1);
                break;
            case VariableTypeEnum.Decimal:
                if (LimitLower is null || LimitUpper is null)
                {
                    CurrentValue = null;
                    throw new InvalidOperationException("Required parameters are invalid: LimitLower or LimitUpper is null.");
                }
                double limitLower2 = LimitLower ?? 0;
                double limitUpper2 = LimitUpper ?? 0;
                int roundingPlaces = RoundingPlaces ?? 4;
                double randomVal = random.NextDouble();
                double range = limitUpper2 - limitLower2;
                double randomDouble = limitLower2 + (range * randomVal);
                randomDouble = (double)Math.Round((decimal)randomDouble, roundingPlaces);
                CurrentValue = randomDouble;
                break;
            case VariableTypeEnum.Choice:
                if (Choices is null)
                {
                    CurrentValue = null;
                    throw new InvalidOperationException("Required parameters are invalid: Choices is null.");
                }
                int randomIdx = random.Next(0, Choices.Length);
                CurrentValue = Choices[randomIdx];
                break;
        }
    }
}

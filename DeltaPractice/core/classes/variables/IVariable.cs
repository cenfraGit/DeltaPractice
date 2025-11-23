namespace core.classes.variables;

public enum VariableType
{
    Integer,
    Decimal,
    Choice
}

/// <summary>
/// A variable represents a dynamic value in the problem. can be used to
/// generate dynamic questions that hold different answers based on the
/// random value of the variable.
/// </summary>
public interface IVariable
{
    object Value { get; }
    VariableType Type { get; }
    void Recalculate();
}

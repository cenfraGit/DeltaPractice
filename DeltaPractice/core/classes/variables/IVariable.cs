namespace core.classes.variables;

/// <summary>
/// A variable represents a dynamic value in the problem. can be used to
/// generate dynamic questions that hold different answers based on the
/// random value of the variable.
/// </summary>
internal interface IVariable<T>
{
    public T Value { get; set; }
    public void Recalculate();
}

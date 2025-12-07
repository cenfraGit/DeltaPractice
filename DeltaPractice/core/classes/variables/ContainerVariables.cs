using System.Collections;

namespace core.classes.variables;

public class ContainerVariables : IRecalculable, IEnumerable<KeyValuePair<string, IVariable>>
{
  private Dictionary<string, IVariable> Variables { get; set; }

  public ContainerVariables()
  {
    this.Variables = new();
  }

  public void Add(string name, IVariable variable)
  {
    this.Variables.Add(name, variable);
  }

  public void Recalculate()
  {
    foreach (var (key, value) in this.Variables)
      value.Recalculate();
  }

  public IEnumerator<KeyValuePair<string, IVariable>> GetEnumerator()
  {
    return this.Variables.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}
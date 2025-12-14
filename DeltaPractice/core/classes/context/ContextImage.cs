using core.classes.variables;

namespace core.classes.context;

public class ContextImage : IRecalculable
{
  public string Value { get; set; }

  public ContextImage(string value)
  {
    this.Value = value; // serialized image
  }

  public void Recalculate()
  {
    
  }
}
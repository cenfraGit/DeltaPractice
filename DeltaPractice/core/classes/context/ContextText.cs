using core.classes.variables;
using core.utils.text;

namespace core.classes.context;

public class ContextText : IRecalculable
{
  public ContainerVariables Variables { get; set; }
  public string Value { get; set; }

  public ContextText(ContainerVariables variables, string text)
  {
    this.Variables = variables;
    this.Value = text;
  }

  public void Recalculate()
  {
    TextUtils.ReplaceVariables(Variables, Value);
  }
}
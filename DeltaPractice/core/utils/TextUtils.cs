using core.classes.variables;

namespace core.utils.text;

public static class TextUtils
{
  public static string ReplaceVariables(ContainerVariables variables, string text)
  {
    string newText = text;
    foreach (var (varName, varValue) in variables)
    {
      if (varValue.Value is null)
        continue;

      newText = newText.Replace($"[{varName}]", varValue.Value.ToString());
    }
    return newText;
  }
}

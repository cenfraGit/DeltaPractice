using core.classes.variables;

namespace core.classes.context;

public class Context : IRecalculable
{
  private ContainerVariables Variables { get; init; }
  public List<IRecalculable> Elements { get; set; }

  // text
  // images
  // equations
  // 2d/3d graphs
  // sound button

  public Context(ContainerVariables variables)
  {
    this.Variables = variables;
    this.Elements = new();
  }

  public void Add(IRecalculable element)
  {
    Elements.Add(element);
  }

  public void Recalculate()
  {
    foreach (var element in this.Elements)
      element.Recalculate();
  }
}

using core.classes.context;
using core.classes.variables;
using core.classes.questions;

namespace core.classes;

public class Problem : IRecalculable
{
  public ContainerVariables Variables { get; set; }
  public Context Context { get; set; }
  public ContainerQuestions Questions { get; set; }

  public Problem(
    ContainerVariables variables,
    Context context,
    ContainerQuestions questions)
  {
    this.Variables = variables;
    this.Context = context;
    this.Questions = questions;
  }

  public void Recalculate()
  {

  }
}

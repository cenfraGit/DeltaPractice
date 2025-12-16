namespace core.classes;

public enum PracticeMode
{
  Normal,
  Test
}

public class Practice
{
  public PracticeMode PracticeMode { get; set; } = PracticeMode.Normal;
  public int PracticeAmount { get; set; } = 5;
  public int Correct { get; set; } = 0;
  public int Incorrect { get; set; } = 0;
  public int CurrentAmount { get { return Correct + Incorrect; } }
  public void Reset() { Correct = 0; Incorrect = 0; }

  public Dictionary<string, Problem> Problems { get; set; } = [];

  public void AddProblem(string name, Problem problem)
  {
    Problems.Add(name, problem);
  }

  public Problem GetProblem()
  {
    Random random = new();
    int idx = random.Next(0, Problems.Count);
    KeyValuePair<string, Problem> problem = Problems.ElementAt(idx);
    problem.Value.Recalculate();
    return problem.Value;
  }
}

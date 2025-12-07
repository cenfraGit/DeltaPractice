using System.Collections;

namespace core.classes.questions;

public class ContainerQuestions : IRecalculable, IEnumerable<KeyValuePair<string, AQuestion>>
{
  private Dictionary<string, AQuestion> Questions { get; set; }

  public ContainerQuestions()
  {
    this.Questions = new();
  }

  public void Add(string name, AQuestion question)
  {
    this.Questions.Add(name, question);
  }

  public void Recalculate()
  {
    foreach (var (key, value) in this.Questions)
      value.Recalculate();
  }

  public IEnumerator<KeyValuePair<string, AQuestion>> GetEnumerator()
  {
    return this.Questions.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}
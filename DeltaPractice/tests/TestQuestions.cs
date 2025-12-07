using core.classes.questions;
using core.classes.variables;

namespace tests;

public class QuestionTests
{
  [Fact]
  public void Test_Questions()
  {

    var myVariables = new ContainerVariables();
    myVariables.Add("myVar1", new VariableInteger(5, 10));
    myVariables.Add("myVar2", new VariableDecimal(5, 10));
    myVariables.Add("myVar3", new VariableChoice([10, 20, 30, 12.5]));

    var myAnswers = new Dictionary<string, string>()
        {
            {"myAnswer1", @"Result = 2;"},
            {"myAnswer2", @"Result = 100.00;"},
            {"myAnswer3", "Result = \"hello\";"},
            {"myAnswer4", "Result = \"dog\";"}
        };

    var quest = new QuestionTextBox(
      myVariables,
        "[myVar1] multiplied by 2 is _myAnswer1_ and " +
        "[myVar2] multiplied by 2 is _myAnswer2_.",
        myAnswers);

    // variables should be initialized from the problem,
    // not questions or vars themselves
    // simulate problem initializing (recalculating) variables
    foreach (var (key, value) in myVariables)
    {
      value.Recalculate();
    }

    quest.Recalculate();

    // Console.WriteLine($"----- Original answer scripts -----");
    // foreach (var (key, value) in quest.AnswersScripts)
    // {
    //     Console.WriteLine($"{key} = {value}");
    // }

    // Console.WriteLine($"----- Calculated answers -----");
    // foreach (var (key, value) in quest.AnswersCorrect)
    // {
    //     Console.WriteLine($"{key} = {value}");
    // }

    // Console.WriteLine($"----- User answers -----");
    quest.AnswersUser = new Dictionary<string, object>()
        {
            {"myAnswer1", 2},
            {"myAnswer2", 90},
            {"myAnswer3", " heLlo  "},
            {"myAnswer4", "Dog"}
        };
    // foreach (var (key, value) in quest.AnswersUser)
    // {
    //     Console.WriteLine($"{key} = {value}");
    // }

    // Console.WriteLine($"----- Checking -----");
    quest.Check();

    // Console.WriteLine(quest.Text);
  }
}

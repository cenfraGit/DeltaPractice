using core.classes.variables;
using core.utils.text;

namespace core.classes.questions;

public class QuestionTextBox : AQuestion
{
  public float ErrorPercentageTolerance { get; set; }

  public QuestionTextBox(
    ContainerVariables variables,
    string text,
    Dictionary<string, string> answers,
    float errorPercentageTolerance = 10) : base(text, variables, answers)
  {
    this.ErrorPercentageTolerance = errorPercentageTolerance;

    // the value [myHeight]mm multiplied by two equals _myHeightCalculation_mm.

    // if no __ is specified, the textbox will appear at the end of the question
    // and assigned the first available answer from the list of answers.
  }

  /// <summary>
  /// Returns true if AnswersCorrect and AnswersUser are equal.
  /// </summary>
  public bool Check()
  {
    foreach (var (answerName, answerCorrect) in AnswersCorrect)
    {
      object answerUser = AnswersUser[answerName];

      // ------------------- if int ------------------- //

      if (answerCorrect is int)
        if (!answerCorrect.Equals(answerUser))
          return false;

        // ----------------- if double ----------------- //

        else if (answerCorrect is double)
        {
          // answer is 100
          // 10% error -> 10/100 -> 0.1
          // 100 * 0.1 => 10

          // we want any answer between 90 and 100 to be correct

          double plus_minus = (double)answerCorrect * (ErrorPercentageTolerance / 100);

          double boundaryLower = (double)answerCorrect - plus_minus;
          double boundaryUpper = (double)answerCorrect + plus_minus;

          // if answerUser is int initially, unbox to int first
          // then double
          double answerUserNew;
          if (answerUser is int)
          {
            answerUserNew = (double)(int)answerUser;
          }
          else if (answerUser is double)
          {
            answerUserNew = (double)answerUser;
          }
          else
          {
            return false;
            //continue;
          }

          if (boundaryLower <= answerUserNew &&
              answerUserNew <= boundaryUpper)
          {
            //Console.WriteLine($"answer {answerUser} is correct.");
          }
          else
          {
            return false;
          }
        }

        // ----------------- if string ----------------- //

        else if (answerCorrect is string)
        {
          Console.WriteLine($"{answerCorrect} is string");
          // implement levenshtein distance?
          if (((string)answerCorrect).Trim().ToLower() ==
              ((string)answerUser).Trim().ToLower())
          {
            //Console.WriteLine($"answer {answerUser} is correct");
          }
          else
          {
            return false;
          }

        }
    }
    return true;
  }
}

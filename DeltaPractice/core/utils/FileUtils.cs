using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using core.classes;
using core.classes.variables;
using core.classes.questions;
using core.classes.context;

namespace core.utils.files;

public class FileUtils
{
  private static readonly string DeltaExtension = ".prac";
  public static readonly string DirTempDelta = Path.Combine(Path.GetTempPath(), "DeltaPractice");

  static FileUtils()
  {
    ClearTempFiles();
  }

  // --------------------------------------------------------------------------------
  // temp folder
  // --------------------------------------------------------------------------------

  public static void ClearTempFiles()
  {
    if (Directory.Exists(DirTempDelta))
      Directory.Delete(DirTempDelta, recursive: true);

    Directory.CreateDirectory(DirTempDelta);
  }

  // --------------------------------------------------------------------------------
  // parsing
  // --------------------------------------------------------------------------------

  public static Problem ParseXMLProb(string xmlProblem)
  {
    // load contents into XElement
    XElement problem = XElement.Parse(xmlProblem);

    // fetch variables
    var xmlVariables = problem.Elements("Variables");
    var xmlVariableData = from variableElement in xmlVariables.Elements("Variable")
                       select new
                       {
                         Name = variableElement.Attribute("Name")?.Value,
                         Type = variableElement.Attribute("Type")?.Value,
                         LimitLower = variableElement.Attribute("LimitLower")?.Value,
                         LimitUpper = variableElement.Attribute("LimitUpper")?.Value,
                         Choices = variableElement.Descendants("Choice")
                       };

    ContainerVariables Variables = new();
    foreach (var v in xmlVariableData)
    {
      IVariable variable;
      switch (v.Type)
      {
        case "Integer":
          variable = new VariableInteger(float.Parse(v.LimitLower), float.Parse(v.LimitUpper));
          break;
        case "Decimal":
          variable = new VariableDecimal(float.Parse(v.LimitLower), float.Parse(v.LimitUpper));
          break;
        case "Choice":
          List<object> choices = new();
          foreach (var c in v.Choices)
          {
            if (c is null) continue;
            XAttribute? valueAttr = c.Attribute("Value"); // need to cast to correct object
            if (valueAttr is null) continue;
            choices.Add(valueAttr.Value);
          }
          variable = new VariableChoice(choices.ToArray());
          break;
        default:
          throw new Exception("Invalid variable type.");
      }
      Variables.Add(v.Name, variable);
    }

    // fetch context
    var xmlContextData = problem.Elements("Context").Elements();

    Context Context = new(Variables);
    foreach (var c in xmlContextData)
    {
      IRecalculable element;
      XAttribute? valueAttr;
      switch (c.Name.ToString())
      {
        case "Text":
          valueAttr = c.Attribute("Value");
          if (valueAttr is null) continue;
          element = new ContextText(Variables, valueAttr.Value);
          break;
        case "Image":
          valueAttr = c.Attribute("Value");
          if (valueAttr is null) continue;
          element = new ContextImage(valueAttr.Value);
          break;
        default:
          throw new Exception("Invalid context element.");
      }
      Context.Add(element);
    }

    // fetch questions
    var xmlQuestions = problem.Elements("Questions");
    var xmlQuestionData = from questionElement in xmlQuestions.Elements("Question")
                       select new
                       {
                         Name = questionElement.Attribute("Name")?.Value,
                         Type = questionElement.Attribute("Type")?.Value,
                         Text = questionElement.Attribute("Text")?.Value,
                         Answers = questionElement.Descendants("Answer")
                       };

    ContainerQuestions Questions = new();
    foreach (var question in xmlQuestionData)
    {
      // no question interface
      switch (question.Type)
      {
        case "TextBox":
          Dictionary<string, string> answers = new();
          foreach (var answer in question.Answers)
          {
            if (answer is null) continue;
            XAttribute? valueAttr = answer.Attribute("Name");
            if (valueAttr is null) continue;
            answers.Add(valueAttr.Value, answer.Value); // name and script
          }
          Questions.Add(question.Name, new QuestionTextBox(Variables, question.Text, answers));
          break;
        default:
          throw new Exception("Invalid question type.");
      }
    }
    return new Problem(Variables, Context, Questions);

  }

  // --------------------------------------------------------------------------------
  // zipping
  // --------------------------------------------------------------------------------

  public static void CompressToPrac(string pathInput, string pathOutput)
  {
    ArgumentNullException.ThrowIfNullOrEmpty(pathInput);
    ArgumentNullException.ThrowIfNullOrEmpty(pathOutput);

    string pathOutputNew = Path.ChangeExtension(pathOutput, DeltaExtension);

    if (File.Exists(pathOutputNew))
      File.Delete(pathOutputNew);

    ZipFile.CreateFromDirectory(pathInput, pathOutputNew);
  }

  public static string DecompressPrac(string path)
  {
    ArgumentNullException.ThrowIfNullOrEmpty(path);

    string pathOutput = Path.Combine(DirTempDelta, Path.GetFileNameWithoutExtension(path));
    if (Directory.Exists(pathOutput))
      Directory.Delete(pathOutput, true);

    ZipFile.ExtractToDirectory(path, pathOutput);

    return pathOutput;
  }

  public static Practice ReadPracFile(string path)
  {
    ArgumentNullException.ThrowIfNullOrEmpty(path);

    string tempContents = DecompressPrac(path);

    string[] problemFiles = Directory.GetFiles(tempContents, "*.xml");


    Practice practiceData = new();
    foreach (string filePath in problemFiles)
    {
      string fileContents = File.ReadAllText(filePath);
      Problem problemData = ParseXMLProb(fileContents);
      practiceData.AddProblem(Path.GetFileNameWithoutExtension(filePath), problemData);
    }
    ClearTempFiles();
    return practiceData;
  }

  // --------------------------------------------------------------------------------
  // images
  // --------------------------------------------------------------------------------

  public static string SerializeImage(string pathImage)
  {
    if (File.Exists(pathImage))
    {
      try
      {
        byte[] imageBytes = File.ReadAllBytes(pathImage);
        return Convert.ToBase64String(imageBytes);
      }
      catch (Exception ex)
      {
        // log? rethrow?
        return null;
      }
    }
    return null;
  }

}

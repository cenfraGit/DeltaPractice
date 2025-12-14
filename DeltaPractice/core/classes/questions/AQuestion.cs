using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using core.classes.variables;
using core.utils.text;

namespace core.classes.questions;

public abstract class AQuestion
{
    // a problem may have several questions.

    // variables may get substituted in the question text via []
    // e.g. [varDistance]mm -> 7mm

    // ------------ answer script setup ------------ //

    public class ScriptGlobals
    {
        public object? result { get; set; }
    }

    public Assembly globalsAssembly;
    public ScriptOptions scriptOptions;
    public ScriptGlobals scriptGlobals;

    // ----------- fields and properties ----------- //

    public ContainerVariables Variables { get; set; }

    public Dictionary<string, string> AnswersScripts { get; set; }
    public Dictionary<string, object> AnswersCorrect { get; set; } = [];
    public Dictionary<string, object> AnswersUser    { get; set; } = [];

    private string _textUnformatted;
    public virtual string Text
    {
        get => TextUtils.ReplaceVariables(Variables, _textUnformatted);
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _textUnformatted = value;
        }
    }

    // ---------------- constructor ---------------- //

    public AQuestion(string text,
                     ContainerVariables variables,
                     Dictionary<string, string> answers)
    {
        this.Text = text;
        this.Variables = variables;
        this.AnswersScripts = answers;
    }

    public static ScriptState<object> RunScript(string code,
                                         ScriptOptions options,
                                         ScriptGlobals globals)
    {
        Task<ScriptState<object>> scriptTask = CSharpScript.RunAsync(
            code: code,
            options: options,
            globals: globals,
            globalsType: typeof(AQuestion.ScriptGlobals)
        );
        ScriptState<object> state = scriptTask.GetAwaiter().GetResult();
        return state;
    }

    /// <summary>
    /// Recalculates the value of the answers based on the
    /// current value of the variables.
    /// </summary>
    public void Recalculate()
    {

        // reset environment and clear old correct values
        this.globalsAssembly = typeof(ScriptGlobals).Assembly;
        this.scriptOptions = ScriptOptions.Default
            .WithReferences(globalsAssembly)
            .WithImports("System", "System.Math");
        this.scriptGlobals = new ScriptGlobals();

        AnswersCorrect.Clear();

        foreach (var (key, value) in AnswersScripts)
        {
            // replace the code with the value of the variables
            string newText = TextUtils.ReplaceVariables(Variables, value);

            // run the script to produce the result value
            var state = RunScript(newText, this.scriptOptions, this.scriptGlobals);

            // fetch result variable
            AnswersCorrect.Add(key, this.scriptGlobals.result);

        }
    }
}

using BranchValidator.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace BranchValidator.Services;

public class ScriptService : IScriptService
{
    // TODO: Load the code form the ExpressionFunction class file for execution

    public bool Execute(string script) => CSharpScript.EvaluateAsync<bool>(script).Result;
}

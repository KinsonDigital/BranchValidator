using BranchValidator.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace BranchValidator.Services;

public class ScriptService : IScriptService
{
    public bool Execute(string expression) => CSharpScript.EvaluateAsync<bool>(expression).Result;
}

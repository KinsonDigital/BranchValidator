using System;
using BranchValidator.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace BranchValidator.Services;

public class ScriptService<T> : IScriptService<T>
{
    // TODO: Load the code form the ExpressionFunction class file for execution

    public T Execute(string scriptSrc)
    {
        var options = ScriptOptions.Default;

        var compiledScript = CSharpScript.Create(scriptSrc, options);

        var compiledResult = compiledScript.Compile();

        // TODO: Implement async await.  Maybe?

        return CSharpScript.EvaluateAsync<T>(scriptSrc).Result;
    }
}

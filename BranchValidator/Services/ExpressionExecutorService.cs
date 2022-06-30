// <copyright file="ExpressionExecutorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Text;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class ExpressionExecutorService : IExpressionExecutorService
{
    private const string ExpressionFunctionsClassName = "ExpressionFunctions";
    private const string ExpressionFunctionsScript = $"{ExpressionFunctionsClassName}.cs";
    private const string BranchInjectionPoint = "//<branch-name/>";
    private const string ExpressionInjectionPoint = "//<expression/>";

    private readonly ICSharpMethodService csharpMethodService;
    private readonly IEmbeddedResourceLoaderService<string> resourceLoaderService;
    private readonly IScriptService<(bool result, string[] funcResults)> scriptService;

    // TODO: Need to inject the IScriptService to execute the script once it has been analyzed
    // and the script created

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionExecutorService"/> class.
    /// </summary>
    /// <param name="expressionValidatorService">Validates expressions.</param>
    public ExpressionExecutorService(
        ICSharpMethodService csharpMethodService,
        IEmbeddedResourceLoaderService<string> resourceLoaderService,
        IScriptService<(bool result, string[] funcResults)> scriptService)
    {
        // TODO: null check and unit test these ctor params
        this.csharpMethodService = csharpMethodService;
        this.scriptService = scriptService;
        this.resourceLoaderService = resourceLoaderService;
    }

    /// <inheritdoc/>
    public (bool valid, string msg) Execute(string expression, string branchName)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return (false, "The expression must not be null or empty.");
        }

        if (string.IsNullOrEmpty(branchName))
        {
            return (false, "The branch name must not be null or empty.");
        }

        expression = expression.Trim();

        var script = this.resourceLoaderService.LoadResource(ExpressionFunctionsScript);

        // Inject the branch name
        script = script.Replace(BranchInjectionPoint, branchName);

        var methodNames = this.csharpMethodService.GetMethodNames(nameof(FunctionDefinitions));

        foreach (var methodName in methodNames)
        {
            var expressionFunName = $"{methodName[0].ToLower()}{methodName[1..]}";
            expression = expression.Replace(expressionFunName, $"{ExpressionFunctionsClassName}.{methodName}");
        }

        // Replace the single quotes with double quotes
        expression = expression.Replace("'", "\"");

        // Inject the expression
        script = script.Replace(ExpressionInjectionPoint, $"return ({expression}, {ExpressionFunctionsClassName}.GetFunctionResults());");

        var expressionResult = this.scriptService.Execute(script);

        var msgResultBuilder = new StringBuilder();
        msgResultBuilder.AppendLine("Function Results:");

        foreach (var funcResult in expressionResult.funcResults)
        {
            msgResultBuilder.AppendLine($"\t{funcResult}");
        }

        var msgResult = msgResultBuilder.ToString().TrimEnd('\r', '\n');

        return (expressionResult.result, msgResult);
    }
}

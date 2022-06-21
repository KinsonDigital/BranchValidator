// <copyright file="ExpressionExecutorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class ExpressionExecutorService : IExpressionExecutorService
{
    private const string ExpressionFunctionsClassName = "ExpressionFunctions";
    private const string ExpressionFunctionsScript = $"{ExpressionFunctionsClassName}.cs";
    private const string BranchInjectionPoint = "//<branch-name/>";
    private const string ExpressionInjectionPoint = "//<expression/>";

    private readonly IExpressionValidatorService expressionValidatorService;
    private readonly IFunctionService functionService;
    private readonly ICSharpMethodService csharpMethodService;
    private readonly IEmbeddedResourceLoaderService<string> resourceLoaderService;
    private readonly IScriptService<bool> scriptService;

    // TODO: Need to inject the IScriptService to execute the script once it has been analyzed
    // and the script created

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionExecutorService"/> class.
    /// </summary>
    /// <param name="expressionValidatorService">Validates expressions.</param>
    /// <param name="functionService">Executes functions.</param>
    public ExpressionExecutorService(
        IExpressionValidatorService expressionValidatorService,
        IFunctionService functionService, // TODO: This will be getting removed
        ICSharpMethodService csharpMethodService,
        IEmbeddedResourceLoaderService<string> resourceLoaderService,
        IScriptService<bool> scriptService)
    {
        // TODO: null check and unit test these ctor params
        this.expressionValidatorService = expressionValidatorService;
        this.functionService = functionService;
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

        var validationResult = this.expressionValidatorService.Validate(expression);

        if (validationResult.isValid is false)
        {
            return validationResult;
        }

        var script = this.resourceLoaderService.LoadResource(ExpressionFunctionsScript);

        script = script.Replace(BranchInjectionPoint, branchName);

        var methodNames = this.csharpMethodService.GetMethodNames(nameof(FunctionDefinitions));

        foreach (var methodName in methodNames)
        {
            var expressionFunName = $"{methodName[0].ToString().ToLower()}{methodName.Substring(1, methodName.Length - 1)}";
            expression = expression.Replace(expressionFunName, $"{ExpressionFunctionsClassName}.{methodName}");
        }

        // Replace the single quotes with double quotes
        expression = expression.Replace("'", "\"");

        script = script.Replace(ExpressionInjectionPoint, $"return {expression};");

        var expressionResult = this.scriptService.Execute(script);

        return (expressionResult, expressionResult ? "branch valid" : "branch invalid");
    }
}

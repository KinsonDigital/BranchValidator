// <copyright file="ExpressionExecutorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class ExpressionExecutorService : IExpressionExecutorService
{
    private const string AndOperator = "&&";
    private const string OrOperator = "||";
    private const char LeftParen = '(';
    private const char RightParen = ')';

    private readonly IExpressionValidatorService expressionValidatorService;
    private readonly IFunctionService functionService;

    // TODO: Need to inject the IScriptService to execute the script once it has been analyzed
    // and the script created

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionExecutorService"/> class.
    /// </summary>
    /// <param name="expressionValidatorService">Validates expressions.</param>
    /// <param name="functionService">Executes functions.</param>
    public ExpressionExecutorService(
        IExpressionValidatorService expressionValidatorService,
        IFunctionService functionService)
    {
        this.expressionValidatorService = expressionValidatorService;
        this.functionService = functionService;
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

        var expressionResult = false;

        return (expressionResult, expressionResult ? "branch valid" : "branch invalid");
    }

    /// <summary>
    /// Extracts the list of argument values from an expression that contains a single function.
    /// </summary>
    /// <param name="function">The function containing the arguments.</param>
    /// <returns>The list of argument values.</returns>
    private static IEnumerable<string> ExtractArgs(string function)
    {
        var allFuncParams = function.GetBetween(LeftParen, RightParen);

        return allFuncParams.Contains(',')
            ? allFuncParams.Split(',', StringSplitOptions.TrimEntries)
            : new[] { allFuncParams };
    }
}

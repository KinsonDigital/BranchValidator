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

        var doesNotContainAndOps = expression.DoesNotContain(AndOperator);
        var doesNotContainOrOps = expression.DoesNotContain(OrOperator);

        var doesNotContainsOps = doesNotContainAndOps && doesNotContainOrOps;

        if (doesNotContainsOps)
        {
            var funcName = expression.GetUpToChar(LeftParen);
            var funcResult = ProcessFunc(expression);

            return (funcResult, $"The function '{funcName}' returned a value of '{funcResult.ToString().ToLower()}'.");
        }

        bool ProcessFunc(string function)
        {
            var funcParams = new List<string>();
            funcParams.AddRange(ExtractArgs(function));

            var result = this.functionService.Execute(function.GetUpToChar(LeftParen), funcParams.ToArray()).valid;

            return result;
        }

        bool ProcessOrOperations(string orFunctions)
        {
            var functions = orFunctions.Split(OrOperator, StringSplitOptions.TrimEntries);

            for (var i = 0; i < functions.Length; i++)
            {
                var funcResult = ProcessFunc(functions[i]);

                functions[i] = funcResult.ToString().ToLower();
            }

            return functions.Any(f => f == "true"); // TODO: Create constance for "true" and "false" values
        }

        var processItems = expression.Split(AndOperator, StringSplitOptions.TrimEntries);

        // If there is only one item with or operators in it, then no AND operators existed
        if (processItems.Length == 1 && processItems[0].Contains(OrOperator))
        {
            processItems = expression.Split(OrOperator, StringSplitOptions.TrimEntries);
        }

        for (var i = 0; i < processItems.Length; i++)
        {
            var processItem = processItems[i];
            bool processResult;

            if (processItem.Contains(OrOperator))
            {
                processResult = ProcessOrOperations(processItem);
            }
            else
            {
                processResult = ProcessFunc(processItem);
            }

            processItems[i] = processResult.ToString().ToLower();
        }

        var expressionResult = processItems.All(i => i == "true");

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

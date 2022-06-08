// <copyright file="FuncAnalyzerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <summary>
/// Analyzes a single function to check for invalid function syntax.
/// </summary>
public class FuncAnalyzerService : IAnalyzerService
{
    private const char LeftParen = '(';
    private const char RightParen = ')';
    private const char SingleQuote = '\'';
    private const char DoubleQuote = '"';
    private static readonly char[] Symbols =
    {
        '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '{', '}', '|', ':',
        '"', '<', '>', '?', '`', '-', '=', '[', ']', '\\', ';', '\'', '.', '/', ',',
    };

    private static readonly char[] Numbers =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    };
    private readonly IFunctionService functionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncAnalyzerService"/> class.
    /// </summary>
    /// <param name="functionService">Holds information about the available functions.</param>
    public FuncAnalyzerService(IFunctionService functionService) => this.functionService = functionService;

    /// <summary>
    /// Analyzes the given single <paramref name="function"/> and returns a result.
    /// </summary>
    /// <param name="function">The function to analyze.</param>
    /// <returns>
    ///     Tuple Result:
    /// <list type="bullet">
    ///     <item><c>valid:</c> <c>true</c> if the <paramref name="function"/> syntax is valid.</item>
    ///     <item><c>msg:</c> Additional information about result.</item>
    /// </list>
    /// </returns>
    public (bool valid, string msg) Analyze(string function)
    {
        if (string.IsNullOrEmpty(function))
        {
            return (false, "The expression must have a single valid function.");
        }

        var totalLeftParens = function.Count(c => c == LeftParen);
        var tooManyLeftParens = totalLeftParens > 1;
        var doesNotContainAnyLeftParens = totalLeftParens <= 0;

        if (tooManyLeftParens)
        {
            return (false, "The function signature has too many '(' symbols.  A function signature can only contain a single '('.");
        }

        if (doesNotContainAnyLeftParens)
        {
            return (false, "The function signature is missing a '('.");
        }

        var totalRightParens = function.Count(c => c == RightParen);
        var tooManyRightParens = totalRightParens > 1;
        var doesNotContainAnyRightParens = totalRightParens <= 0;

        if (tooManyRightParens)
        {
            return (false, "The function signature has too many ')' symbols.  A function signature can only contain a single ')'.");
        }

        if (doesNotContainAnyRightParens)
        {
            return (false, "The function signature is missing a ')'.");
        }

        if (function.DoesNotEndWith(RightParen))
        {
            return (false, "The expression must end with a ')'.");
        }

        var funcName = function.GetUpToChar('(');

        if (funcName.Any(c => Symbols.Contains(c) || Numbers.Contains(c)))
        {
            return (false, "The name of a function can only contain lower or upper case letters.");
        }

        if (this.functionService.FunctionNames.DoesNotContain(funcName))
        {
            return (false, $"The function '{funcName}' is not a valid function that can be used.");
        }

        var allFuncParams = function.GetBetween(LeftParen, RightParen);

        // If the function does not contain an argument
        if (string.IsNullOrEmpty(allFuncParams))
        {
            return (false, $"The '{funcName}' function does not contain an argument.");
        }

        var actualFuncParams = allFuncParams.Contains(',')
            ? allFuncParams.Split(',')
            : new[] { allFuncParams };

        var expectedFuncParamCount = this.functionService.GetTotalFunctionParams(funcName);

        // Make sure that the number of params matches for the function
        if (actualFuncParams.Length != expectedFuncParamCount)
        {
            var paramMsg = "Incorrect number of function arguments.";
            paramMsg += $"The function '{funcName}' currently has '{actualFuncParams}' parameters but is expecting '{expectedFuncParamCount}'.";

            return (false, paramMsg);
        }

        for (var i = 0u; i < actualFuncParams.Length; i++)
        {
            var paramPos = i + 1;
            var funcParam = actualFuncParams[i];

            var paramDataType = this.functionService.GetFunctionParamDataType(funcName, paramPos);

            switch (paramDataType)
            {
                case DataTypes.String:
                    var doesNotContainSingleQuotes = funcParam.DoesNotContain(SingleQuote);
                    var doesNotContainDoubleQuotes = funcParam.DoesNotContain(DoubleQuote);

                    if (doesNotContainSingleQuotes && doesNotContainDoubleQuotes)
                    {
                        return (false, $"Parameter '{paramPos}' for function '{funcName}' must be a string data type.");
                    }

                    break;
                case DataTypes.Number:
                    var containsSingleQuotes = funcParam.Contains(SingleQuote);
                    var containsDoubleQuotes = funcParam.Contains(DoubleQuote);

                    if (containsSingleQuotes || containsDoubleQuotes)
                    {
                        return (false, $"Parameter '{paramPos}' for function '{funcName}' must be a number data type.");
                    }

                    // Check that the parameter only contains numbers
                    var numParamIsNotOnlyNumbers = funcParam.Any(c => Numbers.DoesNotContain(c));

                    if (numParamIsNotOnlyNumbers)
                    {
                        return (false, $"Parameter '{paramPos}' for function '{funcName}' must be a number data type.");
                    }

                    break;
            }
        }

        return (true, string.Empty);
    }
}

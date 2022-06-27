// <copyright file="FunctionExtractorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class FunctionExtractorService : IFunctionExtractorService
{
    private const string AndOperator = "&&";
    private const string OrOperator = "||";
    private const char LeftParen = '(';
    private const char RightParen = ')';
    private const char Comma = ',';
    private const char SingleQuote = '\'';
    private const char DoubleQuote = '"';

    /// <inheritdoc/>
    public IEnumerable<string> ExtractNames(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Array.Empty<string>();
        }

        const StringSplitOptions splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        var doesNotContainAnyOperators = expression.Contains(AndOperator) is false && expression.Contains(OrOperator) is false;

        // If no operators exist, then it is a single function expression
        if (doesNotContainAnyOperators)
        {
            return new[] { expression.Split(LeftParen, splitOptions)[0] };
        }

        var sections = SplitIntoSections(expression);

        var funcNames = new List<string>();

        foreach (var section in sections)
        {
            funcNames.Add(section.Split(LeftParen, splitOptions)[0]);
        }

        return funcNames.ToArray();
    }

    /// <inheritdoc/>
    public IEnumerable<string> ExtractFunctions(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Array.Empty<string>();
        }

        var functions = SplitIntoSections(expression);

        return functions;
    }

    /// <inheritdoc/>
    public IEnumerable<string> ExtractArgValues(string functionSignature)
    {
        if (string.IsNullOrEmpty(functionSignature))
        {
            return Array.Empty<string>();
        }

        if (functionSignature.Contains(AndOperator) || functionSignature.Contains(OrOperator))
        {
            return Array.Empty<string>();
        }

        var containsSingleLeftParen = functionSignature.Count(c => c == LeftParen) == 1;
        var containsSingleRightParen = functionSignature.Count(c => c == RightParen) == 1;

        if (containsSingleLeftParen is false || containsSingleRightParen is false)
        {
            return Array.Empty<string>();
        }

        var argValueStr = functionSignature.GetBetween(LeftParen, RightParen);

        if (argValueStr.Contains(Comma))
        {
            return argValueStr.Split(Comma, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        return string.IsNullOrEmpty(argValueStr) ? Array.Empty<string>() : new[] { argValueStr };
    }

    /// <inheritdoc/>
    public IEnumerable<Type> ExtractArgDataTypes(string functionSignature)
    {
        var argValues = ExtractArgValues(functionSignature);

        return argValues.Select(a =>
        {
            var containsSingleQuotes = a.Contains(SingleQuote);
            var containsDoubleQuotes = a.Contains(DoubleQuote);

            if (containsSingleQuotes || containsDoubleQuotes)
            {
                return typeof(string);
            }

            return typeof(uint);
        });
    }

    /// <summary>
    /// Splits the given <paramref name="expression"/> into sections where each section is a function
    /// that includes the function name, the argument value list parenthesis and the argument values.
    /// </summary>
    /// <param name="expression">The expression to split.</param>
    /// <returns>The list of functions.</returns>
    private static IEnumerable<string> SplitIntoSections(string expression)
    {
        const StringSplitOptions splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        var doesNotContainAnyOperators = expression.Contains(AndOperator) is false && expression.Contains(OrOperator) is false;

        // If no operators exist, then it is a single function expression
        if (doesNotContainAnyOperators)
        {
            return new[] { expression };
        }

        var sections = new List<string>();

        var andSections = expression.Split(AndOperator, splitOptions);

        foreach (var section in andSections)
        {
            if (section.Contains(OrOperator))
            {
                var orSections = section.Split(OrOperator, splitOptions);

                sections.AddRange(orSections);
            }
            else
            {
                sections.Add(section);
            }
        }

        return sections.ToArray();
    }
}

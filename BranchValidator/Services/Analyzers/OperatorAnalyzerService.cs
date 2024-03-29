﻿// <copyright file="OperatorAnalyzerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services.Analyzers;

/// <summary>
/// Analyzes a textual expression to validate whether or not it starts or ends with any <c>&&</c> or <c>||</c> operators
/// or operator characters.
/// </summary>
public class OperatorAnalyzerService : IAnalyzerService
{
    private const string AndOperator = "&&";
    private const string OrOperator = "||";
    private const char AndOperatorChar = '&';
    private const char OrOperatorChar = '|';
    private const char LeftParen = '(';
    private const char RightParen = ')';

    /// <summary>
    /// Analyzes the given <paramref name="expression"/> to verify whether or not it has valid operator syntax.
    /// </summary>
    /// <param name="expression">The expression to analyze.</param>
    /// <returns>
    ///     A result from analyzing the expression.
    ///     Tuple.valid = <c>true</c> means it is valid.
    ///     Tuple.msg = A message about the pass or failure.
    /// </returns>
    /// <remarks>
    /// Things to consider:
    /// <list type="number">
    ///     <item>Any beginning or trailing spaces are ignored.</item>
    ///     <item>A null expression will return valid.</item>
    ///     <item>An empty expression will return valid.</item>
    /// </list>
    /// </remarks>
    public (bool valid, string msg) Analyze(string expression)
    {
        var validResult = (true, "expression valid");

        if (string.IsNullOrEmpty(expression))
        {
            return validResult;
        }

        expression = expression.Trim();

        var doesNotContainOrOpChars = expression.DoesNotContain(OrOperatorChar);
        var doesNotContainAndOpChars = expression.DoesNotContain(AndOperatorChar);
        var doesNotContainOpChars = doesNotContainOrOpChars && doesNotContainAndOpChars;
        var containsFunctions = expression.Count(c => c == LeftParen) >= 2 &&
                                expression.Count(c => c == RightParen) >= 2;

        if (doesNotContainOpChars && containsFunctions)
        {
            return (false, $"Expression functions must be separated by '{AndOperator}' or '{OrOperator}' operators.");
        }

        if (doesNotContainOpChars && !containsFunctions)
        {
            return validResult;
        }

        var startsWithAndOp = expression.StartsWith(AndOperatorChar);
        var endsWithAndOp = expression.EndsWith(AndOperatorChar);

        if (startsWithAndOp || endsWithAndOp)
        {
            return (false, $"Cannot start or end an expression with an '{AndOperator}' operator or '{AndOperatorChar}' character.");
        }

        var startsWithOrOp = expression.StartsWith(OrOperatorChar);
        var endsWithOrOp = expression.EndsWith(OrOperatorChar);

        if (startsWithOrOp || endsWithOrOp)
        {
            return (false, $"Cannot start or end an expression with an '{OrOperator}' operator or '{OrOperatorChar}' character.");
        }

        var totalOrOpChars = expression.Count(c => c == OrOperatorChar);
        var containsOddNumberOfOrOpChars = totalOrOpChars % 2 != 0;

        if (containsOddNumberOfOrOpChars)
        {
            return (false, "Expression is missing an '|' operator.");
        }

        var totalAndOpChars = expression.Count(c => c == AndOperatorChar);
        var containsOddNumberOfAndOpChars = totalAndOpChars % 2 != 0;

        if (containsOddNumberOfAndOpChars)
        {
            return (false, "Expression is missing an '&' operator.");
        }

        // If all of the OR operator characters are not next to each other
        if (expression.Count(OrOperator) != totalOrOpChars / 2)
        {
            return (false, "OR operators must be 2 consecutive '|' symbols.");
        }

        // If all of the AND operator characters are not next to each other
        if (doesNotContainAndOpChars is false && expression.Count(AndOperator) != totalAndOpChars / 2)
        {
            return (false, "AND operators must be 2 consecutive '&' symbols.");
        }

        return AnalyzeOperatorsBetweenFunctions(expression);
    }

    /// <summary>
    /// Analyzes the operators between the functions in the given <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">The expression to analyze.</param>
    /// <returns>
    ///     A result from analyzing the expression.
    ///     <para>
    ///         <c>Tuple.valid</c> = <c>true</c> means the operators ar valid.
    ///     </para>
    ///     <para>
    ///         <c>Tuple.msg</c> = A message about the pass or failure.
    ///     </para>
    /// </returns>
    private (bool valid, string msg) AnalyzeOperatorsBetweenFunctions(string expression)
    {
        var validResult = (true, "expression valid");

        var possibleRanges = new List<(int start, int end)>();

        var startPos = expression.IndexOf(RightParen);
        var endPos = expression.IndexOf(LeftParen, startPos == -1 ? 0 : startPos);

        possibleRanges.Add((startPos, endPos));

        while (startPos != -1 && endPos != -1)
        {
            startPos = expression.IndexOf(RightParen, endPos);
            endPos = expression.IndexOf(LeftParen, startPos == -1 ? 0 : startPos);

            possibleRanges.Add((startPos, endPos));
        }

        // Filter out ranges that have a negative start or end
        possibleRanges = possibleRanges.Where(r => r.start != -1 && r.end != -1).ToList();

        // Filter out invalid ranges
        var rangesWithNoOpsBetween = (from r in possibleRanges
            where expression.IsNotBetween(AndOperator, (uint)r.start, (uint)r.end) &&
                  expression.IsNotBetween(OrOperator, (uint)r.start, (uint)r.end)
            select r).ToArray().Length > 0;

        return rangesWithNoOpsBetween
            ? (false, $"Expression functions must be separated by '{AndOperator}' or '{OrOperator}' operators.")
            : validResult;
    }
}

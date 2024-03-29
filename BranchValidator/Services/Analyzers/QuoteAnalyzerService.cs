﻿// <copyright file="QuoteAnalyzerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services.Analyzers;

/// <summary>
/// Analyzes a textual expression to validate that an even number of single or double quotes exist.
/// </summary>
public class QuoteAnalyzerService : IAnalyzerService
{
    private const char LeftParen = '(';
    private const char RightParen = ')';
    private const char SingleQuote = '\'';
    private const char DoubleQuote = '"';

    /// <summary>
    /// Analyzes the given <paramref name="expression"/> to verify whether or not it has only single or double quotes,
    /// and verifies that the total count of quotes are even in number.
    /// </summary>
    /// <param name="expression">The expression to analyze.</param>
    /// <returns>
    ///     A result from analyzing the expression.
    ///     <para>
    ///         <c>Tuple.valid</c> = <c>true</c> means it is valid.
    ///     </para>
    ///     <para>
    ///         <c>Tuple.msg</c> = A message about the pass or failure.
    ///     </para>
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
        var validResult = (true, string.Empty);

        if (string.IsNullOrEmpty(expression))
        {
            return validResult;
        }

        if (expression.All(c => c is not SingleQuote && c is not DoubleQuote))
        {
            return validResult;
        }

        var containsSingleQuotes = expression.Contains(SingleQuote);
        var containsDoubleQuotes = expression.Contains(DoubleQuote);

        if (containsSingleQuotes && containsDoubleQuotes)
        {
            return (false, "Cannot use both single and double quotes in an expression.");
        }

        expression = expression.Trim();

        var containsOddNumOfSingleQuotes = expression.Count(c => c == SingleQuote) % 2 != 0;
        if (containsSingleQuotes && containsOddNumOfSingleQuotes)
        {
            return (false, "Expression missing a single quote.");
        }

        var containsOddNumOfDoubleQuotes = expression.Count(c => c == DoubleQuote) % 2 != 0;
        if (containsDoubleQuotes && containsOddNumOfDoubleQuotes)
        {
            return (false, "Expression missing a double quote.");
        }

        var onlySingleQuotes = containsSingleQuotes && containsDoubleQuotes is false;
        var onlyDoubleQuotes = containsSingleQuotes is false && containsDoubleQuotes;

        var quoteToCheck = onlySingleQuotes && onlyDoubleQuotes is false
            ? '\''
            : '"';

        var anyQuotesNotBetween = expression.AnyNotBetween(quoteToCheck, LeftParen, RightParen);

        return anyQuotesNotBetween
            ? (false, "Single and double quotes must only exist inside of a function argument list.")
            : validResult;
    }
}

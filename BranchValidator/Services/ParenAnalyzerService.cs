// <copyright file="ParenAnalyzerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <summary>
/// Analyzes a textual expression to validate that it does not start or end with any && or || operators
/// or operator characters.
/// </summary>
public class ParenAnalyzerService : IAnalyzerService
{
    private const char LeftParen = '(';
    private const char RightParen = ')';

    /// <summary>
    /// Analyzes the given <paramref name="expression"/> to verify.
    /// </summary>
    /// <param name="expression">The expression to analyze.</param>
    /// <returns>
    ///     A result from analyzing the expression.
    ///     Tuple.valid = <c>true</c> means it is valid.
    ///     Tuple.msg = a message about the pass or failure.
    /// </returns>
    /// <remarks>
    /// Things to consider:
    /// <list type="number">
    ///     <item>Any beginning or trailing spaces are ignored.</item>
    ///     <item>A null expression will return invalid.</item>
    ///     <item>An empty expression will return invalid.</item>
    /// </list>
    /// </remarks>
    public (bool valid, string msg) Analyze(string expression)
    {
        var validResult = (true, string.Empty);

        var nullOrEmptyExpression = string.IsNullOrEmpty(expression);
        var noParensExist = nullOrEmptyExpression || expression.Any(c => c is LeftParen or RightParen) is false;

        if (nullOrEmptyExpression || noParensExist)
        {
            return (false, "The expression must have at least one function.");
        }

        if (expression.StartsWith(LeftParen) || expression.StartsWith(RightParen))
        {
            return (false, $"The expression cannot start with a '{LeftParen}' or '{RightParen}' parenthesis.");
        }

        if (expression.EndsWith(LeftParen))
        {
            return (false, $"The expression cannot end with a '{LeftParen}'.");
        }

        var totalLeftParens = expression.Count(c => c == LeftParen);
        var totalRightParens = expression.Count(c => c == RightParen);

        if (totalLeftParens != totalRightParens)
        {
            var paren = totalLeftParens < totalRightParens ? LeftParen : RightParen;

            return (false, $"The expression is missing a '{paren}'.");
        }

        // Verify that the first right paren is not positionally before the left paren
        return expression.IndexOf(RightParen) < expression.IndexOf(LeftParen)
            ? (false, "A function parameter list cannot start with a ')'.")
            : validResult;
    }
}

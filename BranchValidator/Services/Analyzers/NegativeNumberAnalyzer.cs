// <copyright file="NegativeNumberAnalyzer.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services.Analyzers;

/// <inheritdoc/>
public class NegativeNumberAnalyzer : IAnalyzerService
{
    /// <summary>
    /// Analyzes the given <paramref name="functionWithArgValues"/> and verifies if any of the
    /// argument values contains a negative number.
    /// </summary>
    /// <param name="functionWithArgValues">A <c>string</c> containing a function with argument values.</param>
    /// <returns>
    ///     A result from analyzing the expression.
    ///     <para>
    ///         <c>Tuple.valid</c> = <c>true</c> means it is valid.
    ///     </para>
    ///     <para>
    ///         <c>Tuple.msg</c> = A message about the pass or failure.
    ///     </para>
    /// </returns>
    public (bool valid, string msg) Analyze(string functionWithArgValues)
    {
        var argValueStr = functionWithArgValues.GetBetween('(', ')');

        var argValues = argValueStr.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var containsNegativeNumber =
            argValues.Where(argValue => !argValue.Contains('\'') && !argValue.Contains('"'))
            .Any(argValue => argValue.Contains('-'));

        return containsNegativeNumber
            ? (false, "Negative number argument values not aloud.")
            : (true, string.Empty);
    }
}

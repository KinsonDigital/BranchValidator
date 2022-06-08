// <copyright file="IAnalyzerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services;

/// <summary>
/// Analyzes expressions.
/// </summary>
public interface IAnalyzerService
{
    /// <summary>
    /// Analyzes the given <paramref name="expression"/> and returns a result.
    /// </summary>
    /// <param name="expression">The expression to analyze.</param>
    /// <returns>
    ///     Tuple Result:
    /// <list type="bullet">
    ///     <item><c>valid:</c> <c>true</c> if the <paramref name="expression"/> is valid.</item>
    ///     <item><c>msg:</c> Additional information about result.</item>
    /// </list>
    /// </returns>
    (bool valid, string msg) Analyze(string expression);
}

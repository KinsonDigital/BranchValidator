// <copyright file="IExpressionExecutorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services;

/// <summary>
/// Executes expressions.
/// </summary>
public interface IExpressionExecutorService
{
    /// <summary>
    /// Execute the given <paramref name="expression"/> against the given <paramref name="branchName"/>.
    /// </summary>
    /// <param name="expression">The expression to execute.</param>
    /// <param name="branchName">The branch name used in each function in the <paramref name="expression"/>.</param>
    /// <returns>
    ///     Tuple Result:
    /// <list type="bullet">
    ///     <item><c>valid:</c> <c>true</c> if the entire expression evaluates to <c>true</c> meaning the branch is valid.</item>
    ///     <item><c>msg:</c> Additional information about the execution.</item>
    /// </list>
    /// </returns>
    (bool valid, string msg) Execute(string expression, string branchName);
}

﻿// <copyright file="IExpressionExecutorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Executes expressions.
/// </summary>
public interface IExpressionExecutorService
{
    /// <summary>
    /// Executes the given <paramref name="expression"/> against the given <paramref name="branchName"/>.
    /// </summary>
    /// <param name="expression">The expression to execute.</param>
    /// <param name="branchName">The branch name used in each function in the <paramref name="expression"/>.</param>
    /// <returns>
    ///     Tuple Result:
    /// <list type="bullet">
    ///     <item><c>valid:</c> <c>true</c> if the entire branch is valid.</item>
    ///     <item><c>msg:</c> Additional information about the execution.</item>
    /// </list>
    /// </returns>
    (bool valid, string msg) Execute(string expression, string branchName);
}

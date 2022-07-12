// <copyright file="IScriptService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Executes scripts and returns a result.
/// </summary>
/// <typeparam name="T">The type of result to return.</typeparam>
public interface IScriptService<out T>
{
    /// <summary>
    /// Executes the given script source code.
    /// </summary>
    /// <param name="scriptSrc">The source code to execute.</param>
    /// <returns>The result of type <typeparamref name="T"/> returned from the script.</returns>
    T Execute(string scriptSrc);
}

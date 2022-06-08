// <copyright file="IFunctionService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Executes specialized functions from an expression.
/// </summary>
public interface IFunctionService
{
    /// <summary>
    /// Gets a list of available functions names.
    /// </summary>
    ReadOnlyCollection<string> FunctionNames { get; }

    /// <summary>
    /// Gets a list of available function signatures.
    /// </summary>
    ReadOnlyCollection<string> FunctionSignatures { get; }

    /// <summary>
    /// Gets the total number of parameters for a function that matches the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <returns>The number of parameters of the function.</returns>
    uint GetTotalFunctionParams(string name);

    /// <summary>
    /// Gets the data type for a parameter in the given <paramref name="paramPos"/> for a function with the given <paramref name="functionName"/>.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="paramPos">The parameter position.</param>
    /// <returns>The data type of the parameter.</returns>
    DataTypes GetFunctionParamDataType(string functionName, uint paramPos);

    /// <summary>
    /// Executes a function that matches the given <paramref name="functionName"/> and uses the given <paramref name="argValues"/>.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="argValues">The list of argument values to send into the function.</param>
    /// <returns>The function execution result.</returns>
    (bool valid, string msg) Execute(string functionName, params string[]? argValues);
}

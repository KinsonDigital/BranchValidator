// <copyright file="IMethodExecutor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Executes a method on an object.
/// </summary>
public interface IMethodExecutor
{
    /// <summary>
    /// Executes a method with the given <paramref name="name"/> on the given <paramref name="obj"/>
    /// using the given <paramref name="argValues"/>.
    /// </summary>
    /// <param name="obj">The object that contains the method.</param>
    /// <param name="name">The name of the method.</param>
    /// <param name="argValues">The argument values to send into the method.</param>
    /// <returns>The result of the method execution.</returns>
    (bool result, string method) ExecuteMethod(object obj, string name, string[]? argValues);
}

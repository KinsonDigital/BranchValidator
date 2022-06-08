// <copyright file="IActionOutputService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Exceptions;

namespace BranchValidator.Services;

/// <summary>
/// Provides the ability to set the GitHub action output.
/// </summary>
public interface IActionOutputService
{
    /// <summary>
    /// Sets the output with the given <paramref name="name"/> to the given <paramref name="value"/>.
    /// </summary>
    /// <param name="name">The name of the output.</param>
    /// <param name="value">The value of the output.</param>
    /// <exception cref="NullOrEmptyStringArgumentException">
    ///     Thrown if the <paramref name="name"/> parameter is null or empty.
    /// </exception>
    void SetOutputValue(string name, string value);
}

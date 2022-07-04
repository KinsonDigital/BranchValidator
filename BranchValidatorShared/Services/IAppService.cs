// <copyright file="IAppService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidatorShared.Services;

/// <summary>
/// Provides application type functionality.
/// </summary>
public interface IAppService
{
    /// <summary>
    /// Exits the application with the given exit <paramref name="code"/>.
    /// </summary>
    /// <param name="code">The exit code to use.</param>
    void Exit(int code);

    /// <summary>
    /// Exits the application with no error code.
    /// </summary>
    void ExitWithNoError();

    /// <summary>
    /// Exists the application with the given error.
    /// </summary>
    /// <param name="exception">The error message.</param>
    void ExitWithException(Exception exception);
}

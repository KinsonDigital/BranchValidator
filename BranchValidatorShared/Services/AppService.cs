// <copyright file="AppService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace BranchValidatorShared.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class AppService : IAppService
{
    private readonly IConsoleService consoleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppService"/> class.
    /// </summary>
    /// <param name="consoleService">Writes to the console.</param>
    public AppService(IConsoleService consoleService) => this.consoleService = consoleService;

    /// <inheritdoc/>
    public void Exit(int code)
    {
#if DEBUG // Kept here to pause console for debugging purposes
        this.consoleService.PauseConsole();
#endif
        this.consoleService.WriteLine($"Exit Code: {code}");
        Environment.Exit(code);
    }

    /// <inheritdoc/>
    public void ExitWithNoError() => Exit(0);

    /// <inheritdoc/>
    public void ExitWithException(Exception exception)
    {
        this.consoleService.BlankLine();
        this.consoleService.WriteError(exception.Message);
        this.consoleService.BlankLine();
        Exit(exception.HResult);
    }
}

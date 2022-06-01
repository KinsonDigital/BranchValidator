// <copyright file="GitHubConsoleService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class GitHubConsoleService : IGitHubConsoleService
{
    /// <inheritdoc/>
    public void Write(string value, bool newLineAfter)
    {
        Console.Write($"{value}");

        if (newLineAfter)
        {
            Console.WriteLine();
        }
    }

    /// <inheritdoc/>
    public void WriteLine(string value) => Console.WriteLine(value);

    /// <inheritdoc/>
    public void WriteLine(uint tabs, string value)
    {
        var allTabs = string.Empty;

        for (var i = 0; i < tabs; i++)
        {
            allTabs += "\t";
        }

        Console.WriteLine($"{allTabs}{value}");
    }

    /// <inheritdoc/>
    public void BlankLine() => Console.WriteLine();

    /// <inheritdoc/>
    public void StartGroup(string name) => Console.WriteLine($"::group::{(string.IsNullOrEmpty(name) ? "Group" : name)}");

    /// <inheritdoc/>
    public void EndGroup() => Console.WriteLine("::endgroup::");

    /// <inheritdoc/>
    public void WriteError(string value)
    {
        var currentClr = Console.ForegroundColor;

#if DEBUG
        Console.ForegroundColor = ConsoleColor.Red;
#endif

        Console.WriteLine($"::error::{value}");
        Console.ForegroundColor = currentClr;
    }

#if DEBUG
    /// <inheritdoc/>
    public void PauseConsole() => Console.ReadKey();
#endif
}

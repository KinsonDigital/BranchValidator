// <copyright file="GitHubConsoleService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class GitHubConsoleService : IGitHubConsoleService
{
    private const char GroupExpanded = '▼';

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
    public void StartGroup(string name)
    {
        // ReSharper disable once RedundantAssignment
        var groupStart = "::group::";

#if DEBUG
        groupStart = $"{GroupExpanded} ";
#endif

        Console.WriteLine($"{groupStart}{(string.IsNullOrEmpty(name) ? "Group" : name)}");
    }

    /// <inheritdoc/>
    public void EndGroup()
    {
#if RELEASE
        Console.WriteLine("::endgroup::");
#endif
    }

    /// <inheritdoc/>
    public void WriteGroup(string title, string content)
    {
        StartGroup(title);
        WriteLine(content);
        EndGroup();
    }

    /// <inheritdoc/>
    public void WriteGroup(string title, string[] contentLines)
    {
        StartGroup(title);

        for (var i = 0; i < contentLines.Length; i++)
        {
            var newContentLine = contentLines[i];
#if DEBUG
            // Add a tab character if in debug mode
            newContentLine = $"|{(i == contentLines.Length - 1 ? "__" : string.Empty)}\t{newContentLine}";
#endif
            WriteLine(newContentLine);
        }

        EndGroup();
    }

    /// <inheritdoc/>
    public void WriteError(string value)
    {
        // ReSharper disable once RedundantAssignment
        var errorGroup = "::error::";
        var currentClr = Console.ForegroundColor;

#if DEBUG
        errorGroup = "ERROR > ";
        Console.ForegroundColor = ConsoleColor.Red;
#endif

        Console.WriteLine($"{errorGroup}{value}");
        Console.ForegroundColor = currentClr;
    }

#if DEBUG
    /// <inheritdoc/>
    public void PauseConsole() => Console.ReadLine();
#endif
}

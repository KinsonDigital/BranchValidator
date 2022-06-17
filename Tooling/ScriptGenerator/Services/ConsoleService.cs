// <copyright file="ConsoleService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace ScriptGenerator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class ConsoleService : IConsoleService
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
    public void StartGroup(string name) => Console.WriteLine($"{GroupExpanded}{(string.IsNullOrEmpty(name) ? "Group" : name)}");

    /// <inheritdoc/>
    public void EndGroup() => Console.WriteLine("__");

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
            // Add a tab character if in debug mode
            var newContentLine = $"|{(i == contentLines.Length - 1 ? "__" : string.Empty)}\t{contentLines[i]}";
            WriteLine(newContentLine);
        }

        EndGroup();
    }

    /// <inheritdoc/>
    public void WriteError(string value)
    {
        var currentClr = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR > {value}");
        Console.ForegroundColor = currentClr;
    }

#if DEBUG
    /// <inheritdoc/>
    public void PauseConsole() => Console.ReadLine();
#endif
}

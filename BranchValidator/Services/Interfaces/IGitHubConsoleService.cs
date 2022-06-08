// <copyright file="IGitHubConsoleService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Provides helper methods for GitHub console related operations.
/// </summary>
public interface IGitHubConsoleService
{
    /// <summary>
    /// Writes the given <paramref name="value"/> to the console.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <param name="newLineAfter">
    ///     If <b>true</b>, the console will be moved down to the next line after
    ///     the <paramref name="value"/> has been written to the console.
    /// </param>
    void Write(string value, bool newLineAfter = false);

    /// <summary>
    /// Writes the given <paramref name="value"/> to the console on its own line.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void WriteLine(string value);

    /// <summary>
    /// Writes the given <paramref name="value"/> to the console on its own line with the given
    /// number of <paramref name="tabs"/> pre-appended to the <paramref name="value"/>.
    /// </summary>
    /// <param name="tabs">The number of tabs to add before the <paramref name="value"/>.</param>
    /// <param name="value">The value to write.</param>
    void WriteLine(uint tabs, string value);

    /// <summary>
    /// Writes a blank line to the console.
    /// </summary>
    void BlankLine();

    /// <summary>
    /// Starts a group.
    /// </summary>
    /// <param name="name">The name to give the group.</param>
    void StartGroup(string name);

    /// <summary>
    /// Ends a group.
    /// </summary>
    void EndGroup();

    /// <summary>
    /// Creates a textual group in the console window with content and ends the group.
    /// </summary>
    /// <param name="title">The title of the group.</param>
    /// <param name="content">The content to print in the group.</param>
    void WriteGroup(string title, string content);

    /// <summary>
    /// Creates a textual group in the console window with content and ends the group.
    /// </summary>
    /// <param name="title">The title of the group.</param>
    /// <param name="contentLines">The individual lines to write to the console.</param>
    void WriteGroup(string title, string[] contentLines);

    /// <summary>
    /// Writes the given <paramref name="value"/> to the console as an error GitHub action workflow command.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void WriteError(string value);

#if DEBUG
    /// <summary>
    /// Pauses the console.
    /// </summary>
    void PauseConsole();
#endif
}

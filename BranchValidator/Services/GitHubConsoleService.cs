// <copyright file="GitHubConsoleService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using BranchValidatorShared.Services;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class GitHubConsoleService : ConsoleService
{
    /// <inheritdoc/>
    public override void StartGroup(string name)
    {
#if DEBUG
        Console.WriteLine($"::group::{(string.IsNullOrEmpty(name) ? "Group" : name)}");
#else
        base.StartGroup(name);
#endif
    }

    /// <inheritdoc/>
    public override void EndGroup()
    {
#if DEBUG
        base.EndGroup();
#else
        Console.WriteLine("::endgroup::");
#endif
    }

    /// <inheritdoc/>
    public override void WriteError(string value)
    {
#if DEBUG
        base.WriteError(value);
#else
        const string errorGroup = "::error::";
        var currentClr = Console.ForegroundColor;

        Console.WriteLine($"{errorGroup}{value}");
        Console.ForegroundColor = currentClr;
#endif
    }
}

// <copyright file="ActionInputs.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator;

/// <summary>
/// Holds all of the GitHub actions inputs.
/// </summary>
public class ActionInputs
{
    /// <summary>
    /// Gets or sets the name of the branch.
    /// </summary>
    [Option(
        "branch-name",
        Required = true,
        Default = "",
        HelpText = "The name of the GIT branch.")]
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets validation logic.
    /// </summary>
    [Option(
        "validation-logic",
        Required = true,
        Default = "",
        HelpText = "The logic expression to use to validate the branch name.")]
    public string ValidationLogic { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value to trim from the start of the <see cref="BranchName"/> <c>string</c>.
    /// </summary>
    [Option(
        "trim-from-start",
        Required = false,
        Default = "",
        HelpText = "The value to trim from the start of the branch.  This is not case sensitive.")]
    public string TrimFromStart { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether or not the action will fail if the <see cref="BranchName"/> is not valid.
    /// </summary>
    [Option(
        "fail-when-not-valid",
        Required = false,
        Default = true,
        HelpText = "If true, will fail the job if the branch name is not valid.")]
    public bool? FailWhenNotValid { get; set; } = true;
}

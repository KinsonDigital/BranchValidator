// <copyright file="AppInputs.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using CommandLine;

namespace ScriptGenerator;

/// <summary>
/// The console application argument inputs.
/// </summary>
public class AppInputs
{
    /// <summary>
    /// Gets or sets the full file path to the source file for processing.
    /// </summary>
    [Option(
        shortName: 's',
        longName: "source-file",
        Required = true,
        Default = "",
        HelpText = "The full file path to the source file.")]
    public string SourceFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full directory path of where to save the generated script file.
    /// </summary>
    [Option(
        shortName: 'd',
        longName: "dest-dir-path",
        Required = true,
        Default = "",
        HelpText = "The full directory path of where to save the generated script file.")]
    public string DestinationDirPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the source generated script file.
    /// </summary>
    [Option(
        shortName: 'f',
        longName: "file-name",
        Required = true,
        Default = "",
        HelpText = "The name of the source generated script file.")]
    public string FileName { get; set; } = string.Empty;
}

// <copyright file="TextFileLinesLoaderService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;

namespace ScriptGenerator.Services;

/// <summary>
/// Loads textual based files.
/// </summary>
[ExcludeFromCodeCoverage]
public class TextFileLinesLoaderService : IFileLoaderService<string[]>
{
    private readonly IFile file;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextFileLinesLoaderService"/> class.
    /// </summary>
    /// <param name="file">Provides ability to deal with files.</param>
    public TextFileLinesLoaderService(IFile file)
        => this.file = file ?? throw new ArgumentNullException(nameof(file), "The parameter must not be null.");

    /// <inheritdoc/>
    public string[] Load(string filePath) => this.file.ReadAllLines(filePath);
}

// <copyright file="TextFileLoaderService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using ScriptGenerator.Services.Interfaces;

namespace ScriptGenerator.Services;

/// <summary>
/// Loads textual based files.
/// </summary>
[ExcludeFromCodeCoverage]
public class TextFileLoaderService : IFileLoaderService<string>
{
    private readonly IFile file;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextFileLoaderService"/> class.
    /// </summary>
    /// <param name="file">Provides ability to deal with files.</param>
    public TextFileLoaderService(IFile file)
        => this.file = file ?? throw new ArgumentNullException(nameof(file), "The parameter must not be null.");

    /// <inheritdoc/>
    public string Load(string filePath) => this.file.ReadAllText(filePath);
}

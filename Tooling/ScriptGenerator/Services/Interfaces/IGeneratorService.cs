// <copyright file="IGeneratorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScriptGenerator.Services.Interfaces;

/// <summary>
/// Generates scripts.
/// </summary>
public interface IGeneratorService
{
    /// <summary>
    /// Creates a script using the source code at the given <paramref name="srcFilePath"/>
    /// and then saves it at the given <paramref name="destDir"/> directory path with
    /// the given <paramref name="destFileName"/>.
    /// </summary>
    /// <param name="srcFilePath">The script source file path.</param>
    /// <param name="destDir">The destination directory path.</param>
    /// <param name="destFileName">The name of the destination file.</param>
    /// <remarks>
    ///     If the <paramref name="destDir"/> does not exist, it is created.
    /// </remarks>
    void GenerateScript(string srcFilePath, string destDir, string destFileName);
}

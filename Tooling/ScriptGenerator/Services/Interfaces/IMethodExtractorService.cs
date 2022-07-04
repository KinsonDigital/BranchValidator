// <copyright file="IMethodExtractorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScriptGenerator.Services.Interfaces;

/// <summary>
/// Generates script functions.
/// </summary>
public interface IMethodExtractorService
{
    /// <summary>
    /// Generate script functions from the code in the file at the given <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath">The full absolute file path to the code file.</param>
    /// <returns>The script functions code.</returns>
    string Extract(string filePath);
}

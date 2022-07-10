// <copyright file="IRelativePathResolverService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScriptGenerator.Services.Interfaces;

/// <summary>
/// Resolves relative paths to fully qualified ones.
/// </summary>
public interface IRelativePathResolverService
{
    /// <summary>
    /// Resolves the given <paramref name="path"/> if it is a relative path.
    /// </summary>
    /// <param name="path">The path to resolve.</param>
    /// <returns>The resolved path.</returns>
    /// <remarks>
    ///     If the <paramref name="path"/> is an absolute path, then the same path will be returned.
    /// </remarks>
    string Resolve(string path);
}

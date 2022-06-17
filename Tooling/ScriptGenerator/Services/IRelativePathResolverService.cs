// <copyright file="IRelativePathResolverService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScriptGenerator.Services;

/// <summary>
/// Resolves relative paths to fully qualified ones.
/// </summary>
public interface IRelativePathResolverService
{
    string Resolve(string path);
}

// <copyright file="IEmbeddedResourceLoaderService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Loads embedded file resources.
/// </summary>
/// <typeparam name="TResourceType">The type of data being returned from the contents of the embedded resource.</typeparam>
public interface IEmbeddedResourceLoaderService<out TResourceType>
{
    /// <summary>
    /// Loads an embedded resource that matches the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The file name of the embedded resource.</param>
    /// <returns>The content from the embedded resource.</returns>
    TResourceType LoadResource(string name);
}

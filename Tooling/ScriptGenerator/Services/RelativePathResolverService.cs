// <copyright file="RelativePathResolverService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.IO.Abstractions;
using ScriptGenerator.Services.Interfaces;

namespace ScriptGenerator.Services;

/// <inheritdoc/>
public class RelativePathResolverService : IRelativePathResolverService
{
    private const char BackSlash = '\\';
    private const char ForwardSlash = '/';
    private readonly IDirectory directory;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelativePathResolverService"/> class.
    /// </summary>
    /// <param name="directory">Provides directory functionality.</param>
    public RelativePathResolverService(IDirectory directory) => this.directory = directory;

    /// <inheritdoc/>
    public string Resolve(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        path = path.Trim();
        path = path.Replace(BackSlash, ForwardSlash);
        path = path.TrimEnd(ForwardSlash);

        if (path.StartsWith($".{ForwardSlash}"))
        {
            var currentWorkingDir = this.directory.GetCurrentDirectory().Replace(BackSlash, ForwardSlash);

            path = path.TrimStart('.').TrimStart(ForwardSlash);
            path = $"{currentWorkingDir}{ForwardSlash}{path}";
        }
        else if (path.StartsWith($"..{ForwardSlash}"))
        {
            var parentDirCount = path.Split(ForwardSlash).Count(i => i == "..");
            var currentWorkingDir = this.directory.GetCurrentDirectory().Replace(BackSlash, ForwardSlash);

            var workingDirSections = currentWorkingDir.Split(ForwardSlash);
            var ignoredDirs = new List<string>();

            for (var i = 1; i <= parentDirCount; i++)
            {
                ignoredDirs.Add(workingDirSections[^i]);
            }

            var finalWorkingDirs = workingDirSections.Where(d => !ignoredDirs.Contains(d)).ToArray();
            var finalWorkingDir = string.Join(ForwardSlash, finalWorkingDirs);

            path = path.Replace($"..{ForwardSlash}", string.Empty).Replace("..", string.Empty);

            path = $"{finalWorkingDir}{ForwardSlash}{path}";
        }

        return path;
    }
}

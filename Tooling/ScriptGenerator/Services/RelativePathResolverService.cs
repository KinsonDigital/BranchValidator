// <copyright file="RelativePathResolverService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.IO.Abstractions;

namespace ScriptGenerator.Services;

/// <inheritdoc/>
public class RelativePathResolverService : IRelativePathResolverService
{
    private const char BackSlash = '\\';
    private const char ForwardSlash = '/';
    private readonly IDirectory dir;

    public RelativePathResolverService(IDirectory dir) => this.dir = dir;

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
            var currentWorkingDir = this.dir.GetCurrentDirectory().Replace(BackSlash, ForwardSlash);

            path = path.TrimStart('.').TrimStart(ForwardSlash);
            path = $"{currentWorkingDir}{ForwardSlash}{path}";
        }
        else if (path.StartsWith($"..{ForwardSlash}"))
        {
            var parentDirCount = path.Split(ForwardSlash).Count(i => i == "..");
            var currentWorkingDir = this.dir.GetCurrentDirectory().Replace(BackSlash, ForwardSlash);

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

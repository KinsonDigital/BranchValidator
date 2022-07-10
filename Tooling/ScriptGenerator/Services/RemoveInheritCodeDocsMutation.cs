// <copyright file="RemoveInheritCodeDocsMutation.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using ScriptGenerator.Services.Interfaces;

namespace ScriptGenerator.Services;

/// <summary>
/// Removes any test lines from a <c>string</c> that contain inherit XML documentation code.
/// </summary>
public class RemoveInheritCodeDocsMutation : IStringMutation
{
    /// <inheritdoc/>
    public string Mutate(string value)
    {
        value = value.Replace("\r\n", "\n");
        value = value.Replace("\n\r", "\n");
        value = value.Replace("\r", "\n");

        var lines = value.Split('\n');

        var resultLines = new List<string>();

        foreach (var line in lines)
        {
            if (line.Contains("///") && line.Contains("<inheritdoc") && line.Contains("/>"))
            {
                continue;
            }

            resultLines.Add(line);
        }

        var result = string.Join(Environment.NewLine, resultLines);

        return result;
    }
}

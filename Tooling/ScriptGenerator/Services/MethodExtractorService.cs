// <copyright file="MethodExtractorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Text;
using ScriptGenerator.Exceptions;
using ScriptGenerator.Services.Interfaces;

namespace ScriptGenerator.Services;

/// <inheritdoc/>
public class MethodExtractorService : IMethodExtractorService
{
    private const string ScriptFunctionStart = "//<script-function>";
    private const string ScriptFunctionEnd = "//</script-function>";
    private readonly IFileLoaderService<string[]> fileLoaderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MethodExtractorService"/> class.
    /// </summary>
    /// <param name="fileLoaderService">Loads textual files.</param>
    public MethodExtractorService(IFileLoaderService<string[]> fileLoaderService)
        => this.fileLoaderService = fileLoaderService;

    /// <inheritdoc/>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="filePath"/> is null or empty.</exception>
    /// <exception cref="FileDataDoesNotExistException">Thrown if no data exists in the file.</exception>
    /// <exception cref="InvalidScriptSourceException">Thrown if there is an issue with the script source.</exception>
    public string Extract(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "The parameter must not be null or empty.");
        }

        var scriptStartFound = false;
        var scriptFunctionSrcLines = this.fileLoaderService.Load(filePath);

        if (scriptFunctionSrcLines.Length <= 0)
        {
            throw new FileDataDoesNotExistException($"The file located at '{filePath}' does not contain any data.");
        }

        var totalStartTags = scriptFunctionSrcLines.Count(l => l.Contains(ScriptFunctionStart));
        var totalEndTags = scriptFunctionSrcLines.Count(l => l.Contains(ScriptFunctionEnd));

        // If the source code lines do not have any start or end tags
        if (totalStartTags <= 0 && totalEndTags <= 0)
        {
            var exMsg = $"The contents of the script file '{filePath}' does not contain any function start and stop tag pairs.";
            throw new InvalidScriptSourceException(exMsg);
        }

        // If there are more start tags then end tags
        if (totalStartTags > totalEndTags)
        {
            throw new InvalidScriptSourceException("A script function end tag '//</script-function>' is missing.");
        }

        // If there are more end tags then start tags
        if (totalStartTags < totalEndTags)
        {
            throw new InvalidScriptSourceException("A script function start tag '//<script-function>' is missing.");
        }

        var functions = new Dictionary<string, string[]>();
        var functionLines = new List<string>();

        bool IsMethod(string line)
        {
            return line.TrimStart().Contains("public") &&
                   line.Count(c => c == '(') == 1 &&
                   line.Count(c => c == ')') == 1 &&
                   line.Trim().Split('(')[0].Split(' ').Length == 3;
        }

        foreach (var line in scriptFunctionSrcLines)
        {
            if (scriptStartFound)
            {
                if (line.Contains(ScriptFunctionEnd))
                {
                    scriptStartFound = false;

                    var funcName = IsMethod(functionLines[0])
                        ? functionLines[0].Trim().Split('(')[0].Split(' ')[2]
                        : Guid.NewGuid().ToString();

                    functions.Add(funcName, functionLines.ToArray());
                    functionLines.Clear();
                    continue;
                }

                functionLines.Add(line);
            }
            else
            {
                scriptStartFound = line.Contains(ScriptFunctionStart);
            }
        }

        var result = new StringBuilder();

        for (var i = 0; i < functions.Keys.Count; i++)
        {
            var key = functions.Keys.ToArray()[i];
            var lines = functions[key];

            var funcStr = string.Join(Environment.NewLine, lines);

            result.AppendLine($"{funcStr}{(i < functions.Keys.Count - 1 ? Environment.NewLine : string.Empty)}");
        }

        return result.ToString().TrimEnd();
    }
}

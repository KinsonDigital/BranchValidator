// <copyright file="ArgParsingService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace BranchValidatorShared.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public sealed class ArgParsingService<T> : IArgParsingService<T>
{
    private bool isDisposed;

    /// <inheritdoc/>
    public async Task ParseArguments(
        T inputs,
        IEnumerable<string> args,
        Func<T, Task> onSuccess,
        Action<string[]> onFailure)
    {
        var parser = Parser.Default.ParseArguments(() => inputs, args);

        parser.WithNotParsed(errors =>
        {
            var result = new List<string>();

            foreach (var error in errors)
            {
                if (error is UnknownOptionError unknownOptionError)
                {
                    result.Add($"Unknown action input with the name '{unknownOptionError.Token}'");
                }
                else if (error is MissingRequiredOptionError requiredOptionError)
                {
                    result.Add($"Missing action input '{requiredOptionError.NameInfo.LongName}'.  This input is required.");
                }
                else if (error is MissingValueOptionError missingValueOptionError)
                {
                    result.Add($"The action input '{missingValueOptionError.NameInfo.LongName}' has no value.");
                }
                else
                {
                    result.Add($"An action input error has occurred.{Environment.NewLine}{error}");
                }
            }

            onFailure(result.ToArray());
        });

        await parser.WithParsedAsync(onSuccess);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        Parser.Default.Dispose();

        this.isDisposed = true;
    }
}

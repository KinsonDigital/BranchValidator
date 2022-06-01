// <copyright file="ArgParsingService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public sealed class ArgParsingService : IArgParsingService<ActionInputs>
{
    private bool isDisposed;

    /// <inheritdoc/>
    public async Task ParseArguments(
        ActionInputs inputs,
        IEnumerable<string> args,
        Func<ActionInputs, Task> onSuccess,
        Action<string[]> onFailure)
    {
        var parser = Default.ParseArguments(() => inputs, args);

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

        Default.Dispose();

        this.isDisposed = true;
    }
}

using CommandLine;

namespace ScriptGenerator;

// TODO: Look into maybe using the version from BranchValidator via project reference instead
public class ArgParsingService : IArgParsingService<AppInputs>
{
    private bool isDisposed;

    /// <inheritdoc/>
    public async Task ParseArguments(
        AppInputs inputs,
        IEnumerable<string> args,
        Func<AppInputs, Task> onSuccess,
        Action<string[]> onFailure)
    {
        var parser = Parser.Default.ParseArguments(() => inputs, args);

        parser.WithNotParsed(errors =>
        {
            var result = new List<string>();

            foreach (var error in errors)
            {
                // if (error is UnknownOptionError unknownOptionError)
                // {
                //     result.Add($"Unknown action input with the name '{unknownOptionError.Token}'");
                // }
                // else if (error is MissingRequiredOptionError requiredOptionError)
                // {
                //     result.Add($"Missing action input '{requiredOptionError.NameInfo.LongName}'.  This input is required.");
                // }
                // else if (error is MissingValueOptionError missingValueOptionError)
                // {
                //     result.Add($"The action input '{missingValueOptionError.NameInfo.LongName}' has no value.");
                // }
                // else
                // {
                //     result.Add($"An action input error has occurred.{Environment.NewLine}{error}");
                // }
                result.Add($"An action input error has occurred.{Environment.NewLine}{error}");
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

namespace ScriptGenerator;

/// <summary>
/// Parses arguments and responds based on successful or unsuccessful parsing.
/// </summary>
/// <typeparam name="T">The object holding the action input data.</typeparam>
public interface IArgParsingService<T> : IDisposable
{
    /// <summary>
    /// Parses the given <paramref name="args"/> for the given inputs of type <typeparamref name="T"/>.
    /// Will invoke the given <paramref name="onSuccess"/> delegate if parsing is successful and
    /// invoke the given <paramref name="onFailure"/> when parsing is not successful.
    /// </summary>
    /// <param name="inputs">The inputs object that will hold the values from the given <paramref name="args"/>.</param>
    /// <param name="args">The arguments to parse.</param>
    /// <param name="onSuccess">Invoked when parsing is successful.</param>
    /// <param name="onFailure">Invoked when parsing is not successful.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ParseArguments(T inputs, IEnumerable<string> args, Func<T, Task> onSuccess, Action<string[]> onFailure);
}

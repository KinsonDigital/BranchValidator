namespace ScriptGenerator.Services;

/// <summary>
/// Loads file data.
/// </summary>
/// <typeparam name="T">The type of data to return from a file.</typeparam>
public interface IFileLoaderService<out T>
{
    /// <summary>
    /// Loads the file at the given <paramref name="filePath"/> and returns the data of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="filePath">The full file path to the file.</param>
    /// <returns>The data.</returns>
    T Load(string filePath);
}

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Extracts the names of functions from an expression.
/// </summary>
public interface IFunctionNamesExtractorService
{
    /// <summary>
    /// Extracts only the names from the given <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">The expression to extract from.</param>
    /// <returns>The list of function names in the given <paramref name="expression"/>.</returns>
    /// <remarks>
    ///     If any functions have a mismatch in parenthesis counts, then the function name will be ignored.
    /// </remarks>
    IEnumerable<string> ExtractNames(string expression);
}

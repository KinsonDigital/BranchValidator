// <copyright file="IFunctionExtractorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Extracts different pieces of function signatures from functions in an expression.
/// </summary>
public interface IFunctionExtractorService
{
    /// <summary>
    /// Extracts only the names from the given <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">The expression to extract.</param>
    /// <returns>The list of function names in the given <paramref name="expression"/>.</returns>
    /// <remarks>
    ///     If any functions have a mismatch in the number of parenthesis, then the function name will be ignored.
    /// </remarks>
    IEnumerable<string> ExtractNames(string expression);

    /// <summary>
    /// Extracts all of the function signatures and returns them as a list.
    /// </summary>
    /// <param name="expression">The expression containing the functions.</param>
    /// <returns>The list of full function signatures.</returns>
    /// <remarks>
    /// A function signature is the name, left parenthesis, right parenthesis, and the argument value.
    /// </remarks>
    IEnumerable<string> ExtractFunctions(string expression);

    /// <summary>
    /// Extracts all of the function argument values from a function in the given <paramref name="functionSignature"/>.
    /// </summary>
    /// <param name="functionSignature">The function signature.</param>
    /// <returns>The list of function argument values.</returns>
    IEnumerable<string> ExtractArgValues(string functionSignature);

    /// <summary>
    /// Extracts all of the function argument data types from the given <paramref name="functionSignature"/>.
    /// </summary>
    /// <param name="functionSignature">The function signature that may or may not contain the arguments.</param>
    /// <returns>A list of function argument data types.</returns>
    IEnumerable<Type> ExtractArgDataTypes(string functionSignature);
}

// <copyright file="IExpressionValidatorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services;

/// <summary>
/// Validates expressions by checking for syntax and other related issues.
/// </summary>
public interface IExpressionValidatorService
{
    /// <summary>
    /// Validates the given <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">The expression to validate.</param>
    /// <returns>
    ///     A tuple of the validation results.
    ///
    /// Tuple Members:
    /// <list type="bullet">
    ///     <item>isValid: True if the expression is valid.</item>
    ///     <item>msg: Additional information about the validation.-</item>
    /// </list>
    /// </returns>
    (bool isValid, string msg) Validate(string expression);
}

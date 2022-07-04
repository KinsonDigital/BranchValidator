// <copyright file="IParsingService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Provides parsing functionality.
/// </summary>
public interface IParsingService
{
    /// <summary>
    /// Converts a <c>C#</c> method signature to an expression function signature.
    /// </summary>
    /// <param name="methodSignature">The method signature to convert.</param>
    /// <returns>The expression function equivalent.</returns>
    string ToExpressionFunctionSignature(string methodSignature);
}

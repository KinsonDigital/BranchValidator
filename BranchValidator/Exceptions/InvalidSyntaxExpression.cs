// <copyright file="InvalidSyntaxExpression.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Exceptions;

/// <summary>
/// Occurs when a branch is invalid.
/// </summary>
public class InvalidSyntaxExpression : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidSyntaxExpression"/> class.
    /// </summary>
    public InvalidSyntaxExpression()
        : base("The expression syntax is invalid.") => HResult = 500;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidSyntaxExpression"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidSyntaxExpression(string message)
        : base(message) => HResult = 500;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidSyntaxExpression"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public InvalidSyntaxExpression(string message, Exception innerException)
        : base(message, innerException) => HResult = 500;
}

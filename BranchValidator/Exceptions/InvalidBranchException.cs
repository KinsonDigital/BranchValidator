// <copyright file="InvalidBranchException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Exceptions;

/// <summary>
/// Occurs when a branch is invalid.
/// </summary>
public class InvalidBranchException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidBranchException"/> class.
    /// </summary>
    public InvalidBranchException()
        : base("The branch is invalid.") => HResult = 600;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidBranchException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidBranchException(string message)
        : base(message) => HResult = 600;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidBranchException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public InvalidBranchException(string message, Exception innerException)
        : base(message, innerException) => HResult = 600;
}


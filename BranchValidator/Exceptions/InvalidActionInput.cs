// <copyright file="InvalidActionInput.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Exceptions;

/// <summary>
/// Occurs when a GitHub action input has an invalid value.
/// </summary>
public class InvalidActionInput : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidActionInput"/> class.
    /// </summary>
    public InvalidActionInput()
        : base("The action input value is invalid.") => HResult = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidActionInput"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidActionInput(string message)
        : base(message) => HResult = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidActionInput"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public InvalidActionInput(string message, Exception innerException)
        : base(message, innerException) => HResult = 10;
}

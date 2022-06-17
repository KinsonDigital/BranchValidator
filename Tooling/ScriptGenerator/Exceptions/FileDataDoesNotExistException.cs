// <copyright file="FileDataDoesNotExistException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScriptGenerator.Exceptions;

/// <summary>
/// Occurs when a file does not contain any data.
/// </summary>
public class FileDataDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileDataDoesNotExistException"/> class.
    /// </summary>
    public FileDataDoesNotExistException()
        : base("No file data exists.") => HResult = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileDataDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public FileDataDoesNotExistException(string message)
        : base(message) => HResult = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileDataDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public FileDataDoesNotExistException(string message, Exception innerException)
        : base(message, innerException) => HResult = 10;
}
